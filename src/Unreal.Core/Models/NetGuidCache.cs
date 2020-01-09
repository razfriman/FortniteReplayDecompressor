using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Unreal.Core.Models
{
    public class NetGuidCache
    {
        public Dictionary<uint, NetGuidCacheObject> ObjectLookup { get; private set; } = new Dictionary<uint, NetGuidCacheObject>();
        public Dictionary<string, NetFieldExportGroup> NetFieldExportGroupMap { get; private set; } = new Dictionary<string, NetFieldExportGroup>();
        public Dictionary<uint, NetFieldExportGroup> NetFieldExportGroupIndexToGroup { get; private set; } = new Dictionary<uint, NetFieldExportGroup>();
        //public Dictionary<uint, NetGuidCacheObject> ImportedNetGuids { get; private set; } = new Dictionary<uint, NetGuidCacheObject>();
        public Dictionary<uint, string> NetGuidToPathName { get; private set; } = new Dictionary<uint, string>();
        public NetFieldExportGroup NetworkGameplayTagNodeIndex { get; private set; }

        private Dictionary<uint, NetFieldExportGroup> _archTypeToExportGroup = new Dictionary<uint, NetFieldExportGroup>();
        public Dictionary<uint, NetFieldExportGroup> NetFieldExportGroupMapPathFixed { get; private set; } = new Dictionary<uint, NetFieldExportGroup>();
        private Dictionary<uint, string> _cleanedPaths = new Dictionary<uint, string>();
        private Dictionary<string, string> _cleanedClassNetCache = new Dictionary<string, string>();
        private Dictionary<string, string> _partialPathNames = new Dictionary<string, string>();

        public void ClearCache()
        {
            ObjectLookup.Clear();
            NetFieldExportGroupMap.Clear();
            NetFieldExportGroupIndexToGroup.Clear();
            NetGuidToPathName.Clear();
            _archTypeToExportGroup.Clear();
            NetFieldExportGroupMapPathFixed.Clear();
            _cleanedPaths.Clear();
        }

        public UObject GetObjectFromNetGUID(NetworkGUID netGuid, bool ignoreMustBeMapped)
        {
            if(!netGuid.IsValid())
            {
                return null;
            }

            if(!netGuid.IsDefault())
            {
                return null;
            }

            if(!ObjectLookup.TryGetValue(netGuid.Value, out NetGuidCacheObject cacheObject))
            {
                return null;
            }

            if(cacheObject.IsBroken)
            {
                return null;
            }

            if (cacheObject.IsPending)
            {
                return null;
            }

            if (String.IsNullOrEmpty(cacheObject.PathName))
            {
                return null;
            }

            if(cacheObject.OuterGuid.IsValid())
            {
                if (!ObjectLookup.TryGetValue(cacheObject.OuterGuid.Value, out NetGuidCacheObject outerCacheObject))
                {
                    return null;
                }

                if(outerCacheObject.IsBroken)
                {
                    cacheObject.IsBroken = true;

                    return null;
                }

                UObject objOuter = GetObjectFromNetGUID(outerCacheObject.OuterGuid, ignoreMustBeMapped);

                if(objOuter == null)
                {
                    return null;
                }
            }


            return null;
        }

        public void AddToExportGroupMap(string group, NetFieldExportGroup exportGroup)
        {
            if (NetworkGameplayTagNodeIndex == null && group == "NetworkGameplayTagNodeIndex")
            {
                NetworkGameplayTagNodeIndex = exportGroup;
            }

            NetFieldExportGroupMap[group] = exportGroup;

            //Check if partial path
            foreach(KeyValuePair<string, string> partialRedirectKvp in CoreRedirects.PartialRedirects)
            {
                if(group.StartsWith(partialRedirectKvp.Key))
                {
                    _partialPathNames.Add(group, partialRedirectKvp.Value);
                }
            }
        }

        public NetFieldExportGroup GetNetFieldExportGroup(string pathName)
        {
            if(String.IsNullOrEmpty(pathName))
            {
                return null;
            }

            if(NetFieldExportGroupMap.TryGetValue(pathName, out NetFieldExportGroup netFieldExportGroup))
            {
                return netFieldExportGroup;
            }

            return null;
        }

        public NetFieldExportGroup GetNetFieldExportGroup(uint guid)
        {
            if (!_archTypeToExportGroup.ContainsKey(guid))
            {
                if(!NetGuidToPathName.ContainsKey(guid))
                {
                    return null;
                }

                var path = NetGuidToPathName[guid];

                path = CoreRedirects.GetRedirect(path);

                if(_partialPathNames.TryGetValue(path, out string redirectPath))
                {
                    path = redirectPath;
                }

                if (NetFieldExportGroupMapPathFixed.ContainsKey(guid))
                {
                    _archTypeToExportGroup[guid] = NetFieldExportGroupMapPathFixed[guid];

                    return NetFieldExportGroupMapPathFixed[guid];
                }

                foreach (var groupPathKvp in NetFieldExportGroupMap)
                {
                    var groupPath = groupPathKvp.Key;

                    if (!_cleanedPaths.TryGetValue(groupPathKvp.Value.PathNameIndex, out var groupPathFixed))
                    {
                        groupPathFixed = RemoveAllPathPrefixes(groupPath);
                        _cleanedPaths[groupPathKvp.Value.PathNameIndex] = groupPathFixed;
                    }

                    if (path.Contains(groupPathFixed, StringComparison.Ordinal))
                    {
                        NetFieldExportGroupMapPathFixed[guid] = NetFieldExportGroupMap[groupPath];
                        _archTypeToExportGroup[guid] = NetFieldExportGroupMap[groupPath];

                        return NetFieldExportGroupMap[groupPath];
                    }
                }


                //Try fixing ...
                /*if(path.EndsWith("Set", StringComparison.Ordinal) || path.EndsWith("Component", StringComparison.Ordinal)) //Need to deal with later
                {
                    return null;
                }*/

                var cleanedPath = CleanPathSuffix(path);

                foreach (var groupPathKvp in NetFieldExportGroupMap)
                {
                    var groupPath = groupPathKvp.Key;

                    if (_cleanedPaths.TryGetValue(groupPathKvp.Value.PathNameIndex, out var groupPathFixed))
                    {
                        if (groupPathFixed.Contains(cleanedPath, StringComparison.Ordinal))
                        {
                            NetFieldExportGroupMapPathFixed[guid] = NetFieldExportGroupMap[groupPath];
                            _archTypeToExportGroup[guid] = NetFieldExportGroupMap[groupPath];

                            return NetFieldExportGroupMap[groupPath];
                        }
                    }
                }

                return null;
            }
            else
            {
                return _archTypeToExportGroup[guid];
            }
        }

        public NetFieldExportGroup GetNetFieldExportGroupForClassNetCache(string group)
        {
            if (!_cleanedClassNetCache.TryGetValue(group, out var classNetCachePath))
            {
                classNetCachePath = $"{RemoveAllPathPrefixes(group)}_ClassNetCache";
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

            for(int i = path.Length - 1; i >= 0; i--)
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
            if(toRemove.Length > path.Length)
            {
                return path;
            }

            for(int i = 0; i < toRemove.Length; i++)
            {
                if(path[i] != toRemove[i])
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
            for(int i = path.Length - 1; i >= 0; i--)
            {
                bool isDigit = (path[i] ^ '0') <= 9;
                bool isUnderscore = path[i] == '_';

                if(!isDigit && !isUnderscore)
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
