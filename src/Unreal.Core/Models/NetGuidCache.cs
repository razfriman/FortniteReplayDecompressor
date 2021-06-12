using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Unreal.Core.Extensions;

namespace Unreal.Core.Models
{
    public class NetGuidCache
    {
        public NetFieldExportGroup NetworkGameplayTagNodeIndex { get; private set; }

        public Dictionary<string, NetFieldExportGroup> NetFieldExportGroupMap { get; private set; } = new Dictionary<string, NetFieldExportGroup>();
        public Dictionary<uint, NetFieldExportGroup> NetFieldExportGroupIndexToGroup { get; private set; } = new Dictionary<uint, NetFieldExportGroup>();
        //public Dictionary<uint, NetGuidCacheObject> ImportedNetGuids { get; private set; } = new Dictionary<uint, NetGuidCacheObject>();
        public Dictionary<uint, string> NetGuidToPathName { get; private set; } = new Dictionary<uint, string>();

        public Dictionary<uint, NetFieldExportGroup> NetFieldExportGroupMapPathFixed { get; private set; } = new Dictionary<uint, NetFieldExportGroup>();

        private Dictionary<string, string> _cleanedClassNetCache = new Dictionary<string, string>();
        private Dictionary<string, string> _partialPathNames = new Dictionary<string, string>();
        private Dictionary<uint, NetFieldExportGroup> _archTypeToExportGroup = new Dictionary<uint, NetFieldExportGroup>();
        private HashSet<string> _failedPaths = new HashSet<string>(); //Path names that didn't find an export group

        private Dictionary<string, NetFieldExportGroup> _pathToExportGroup = new Dictionary<string, NetFieldExportGroup>();

        private NetFieldParser _parser;

        internal NetGuidCache(NetFieldParser parser)
        {
            _parser = parser;
        }

        public void ClearCache()
        {
            NetFieldExportGroupMap.Clear();
            NetFieldExportGroupIndexToGroup.Clear();
            NetGuidToPathName.Clear();
            NetFieldExportGroupMapPathFixed.Clear();
            NetworkGameplayTagNodeIndex = null;

            _archTypeToExportGroup.Clear();
            _cleanedClassNetCache.Clear();
            _partialPathNames.Clear();
            _failedPaths.Clear();
            _pathToExportGroup.Clear();
        }

        public void AddToExportGroupMap(string group, NetFieldExportGroup exportGroup)
        {
            if (NetworkGameplayTagNodeIndex == null && group == "NetworkGameplayTagNodeIndex")
            {
                NetworkGameplayTagNodeIndex = exportGroup;
            }

            //Easiest way to do this update
            if(group.EndsWith("ClassNetCache"))
            {
                exportGroup.PathName = RemoveAllPathPrefixes(exportGroup.PathName);
            }

            exportGroup.CleanedPath = RemoveAllPathPrefixes(group);

            /*
            if(NetFieldExportGroupMap.TryGetValue(group, out var temp))
            {
                Console.WriteLine($"Found Duplicate: {group}\n\t- {temp.PathName}\n\t- {exportGroup.PathName}");
            }
            */

            NetFieldExportGroupMap[group] = exportGroup;

            //Console.WriteLine(++count);
            _parser.UpdateExportGroup(exportGroup);

            //Check if partial path
            foreach (KeyValuePair<string, string> partialRedirectKvp in CoreRedirects.PartialRedirects)
            {
                if (group.StartsWith(partialRedirectKvp.Key))
                {
                    _partialPathNames.TryAdd(group, RemoveAllPathPrefixes(partialRedirectKvp.Value));
                    _partialPathNames.TryAdd(RemoveAllPathPrefixes(group), RemoveAllPathPrefixes(partialRedirectKvp.Value));

                    break;
                }
            }
        }

        public NetFieldExportGroup GetNetFieldExportGroupByPath(uint pathIndex)
        {
            NetFieldExportGroupIndexToGroup.TryGetValue(pathIndex, out NetFieldExportGroup group);

            return group;
        }

        public NetFieldExportGroup GetNetFieldExportGroup(string pathName)
        {
            if (String.IsNullOrEmpty(pathName))
            {
                return null;
            }

            if (NetFieldExportGroupMap.TryGetValue(pathName, out NetFieldExportGroup netFieldExportGroup))
            {
                return netFieldExportGroup;
            }

            return null;
        }

        public NetFieldExportGroup GetNetFieldExportGroup(uint guid)
        {
            if (!_archTypeToExportGroup.ContainsKey(guid))
            {
                if (!NetGuidToPathName.ContainsKey(guid))
                {
                    return null;
                }

                var path = NetGuidToPathName[guid];

                //Don't need to recheck. Some export groups are added later though
                if (_failedPaths.Contains(path))
                {
                    return null;
                }

                path = CoreRedirects.GetRedirect(path);

                if (_partialPathNames.TryGetValue(path, out string redirectPath))
                {
                    path = redirectPath;
                }

                if (NetFieldExportGroupMapPathFixed.TryGetValue(guid, out var exportGroup) || _pathToExportGroup.TryGetValue(path, out exportGroup))
                {
                    _archTypeToExportGroup[guid] = exportGroup;

                    return exportGroup;
                }

                /*
                if(NetFieldExportGroupMap.TryGetValue(RemoveAllPathPrefixes(path), out exportGroup))
                {
                    return exportGroup;
                }*/

                string cleaned = RemoveAllPathPrefixes(path);

                foreach (var groupPathKvp in NetFieldExportGroupMap)
                {
                    if (cleaned == groupPathKvp.Value.CleanedPath)
                    {
                        NetFieldExportGroupMapPathFixed[guid] = groupPathKvp.Value;
                        _archTypeToExportGroup[guid] = groupPathKvp.Value;
                        _pathToExportGroup[path] = groupPathKvp.Value;

                        return groupPathKvp.Value;
                    }
                }

                //Try fixing ...
                var cleanedPath = CleanPathSuffix(path);

                foreach (var groupPathKvp in NetFieldExportGroupMap)
                {
                    if (groupPathKvp.Value.CleanedPath.Contains(cleanedPath))
                    {

                        NetFieldExportGroupMapPathFixed[guid] = groupPathKvp.Value;
                        _archTypeToExportGroup[guid] = groupPathKvp.Value;
                        _pathToExportGroup[path] = groupPathKvp.Value;

                        return groupPathKvp.Value;
                    }
                }

                _failedPaths.Add(path);

                return null;
            }
            else
            {
                return _archTypeToExportGroup[guid];
            }
        }

        static int count = 0;

        public NetFieldExportGroup GetNetFieldExportGroupForClassNetCache(string group, bool fullPath = false)
        {
            if (!_cleanedClassNetCache.TryGetValue(group, out var classNetCachePath))
            {
                if (fullPath)
                {
                    classNetCachePath = $"{group}_ClassNetCache";
                }
                else
                {
                    classNetCachePath = $"{RemoveAllPathPrefixes(group)}_ClassNetCache";
                }

                _cleanedClassNetCache[group] = classNetCachePath;
            }

            if (!NetFieldExportGroupMap.ContainsKey(classNetCachePath))
            {
                return default;
            }

            return NetFieldExportGroupMap[classNetCachePath];
        }

        public string RemoveAllPathPrefixes(string path)
        {
            path = RemovePathPrefix(path, "Default__");

            for (int i = path.Length - 1; i >= 0; i--)
            {
                switch (path[i])
                {
                    case '.':
                        return path.Substring(i + 1);
                    case '/':
                        return path;
                }
            }

            return path;
        }

        private string RemovePathPrefix(string path, string toRemove)
        {
            if (toRemove.Length > path.Length)
            {
                return path;
            }

            for (int i = 0; i < toRemove.Length; i++)
            {
                if (path[i] != toRemove[i])
                {
                    return path;
                }
            }

            return path.Substring(toRemove.Length);
        }

        private string RemovePathSuffix(string path, string toRemove)
        {
            if (toRemove.Length > path.Length)
            {
                return path;
            }

            for (int i = 0; i < toRemove.Length; i++)
            {
                int pathIndex = path.Length - toRemove.Length + i;

                if (path[pathIndex] != toRemove[i])
                {
                    return path;
                }
            }

            return path.Substring(0, path.Length - toRemove.Length);
        }

        //Removes all numbers and underscores from suffix
        private string CleanPathSuffix(string path)
        {
            for (int i = path.Length - 1; i >= 0; i--)
            {
                bool isDigit = (path[i] ^ '0') <= 9;
                bool isUnderscore = path[i] == '_';

                if (!isDigit && !isUnderscore)
                {
                    return path.Substring(0, i + 1);
                }
            }

            return path;
        }

        /*
        private string RemovePathSuffix(string path)
        {
            return Regex.Replace(path, @"(_?[0-9]+)+$", "");
        }

        private string RemovePathSuffix(string path, string toRemove)
        {
            return Regex.Replace(path, $@"{toRemove}$", "");
        }
        */
    }
}
