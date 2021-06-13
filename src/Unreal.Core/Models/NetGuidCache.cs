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
        public Dictionary<uint, string> NetGuidToPathName { get; private set; } = new Dictionary<uint, string>();
        private Dictionary<uint, NetFieldExportGroup> _archTypeToExportGroup = new Dictionary<uint, NetFieldExportGroup>();

        private Dictionary<string, string> _cleanedClassNetCache = new Dictionary<string, string>();
        private Dictionary<string, string> _partialPathNames = new Dictionary<string, string>();

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
            NetworkGameplayTagNodeIndex = null;

            _archTypeToExportGroup.Clear();
            _cleanedClassNetCache.Clear();
            _partialPathNames.Clear();
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
                exportGroup.PathName = Utilities.RemoveAllPathPrefixes(exportGroup.PathName);
            }

            exportGroup.CleanedPath = Utilities.RemoveAllPathPrefixes(group);

            NetFieldExportGroupMap[exportGroup.CleanedPath] = exportGroup;

            _parser.UpdateExportGroup(exportGroup);

            //Check if partial path
            foreach (KeyValuePair<string, string> partialRedirectKvp in CoreRedirects.PartialRedirects)
            {
                if (group.StartsWith(partialRedirectKvp.Key, StringComparison.Ordinal))
                {
                    _partialPathNames.TryAdd(group, Utilities.RemoveAllPathPrefixes(partialRedirectKvp.Value));
                    _partialPathNames.TryAdd(Utilities.RemoveAllPathPrefixes(group), Utilities.RemoveAllPathPrefixes(partialRedirectKvp.Value));

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
            if (!_archTypeToExportGroup.TryGetValue(guid, out NetFieldExportGroup group))
            {
                if (!NetGuidToPathName.ContainsKey(guid))
                {
                    return null;
                }

                var path = NetGuidToPathName[guid];

                path = CoreRedirects.GetRedirect(path);

                if (_partialPathNames.TryGetValue(path, out string redirectPath))
                {
                    path = redirectPath;
                }

                if (NetFieldExportGroupMap.TryGetValue(path, out var exportGroup))
                {
                    _archTypeToExportGroup[guid] = exportGroup;

                    return exportGroup;
                }

                return null;
            }
            else
            {
                return group;
            }
        }

        public NetFieldExportGroup GetNetFieldExportGroupForClassNetCache(string group, bool fullPath = false)
        {
            if (!_cleanedClassNetCache.TryGetValue(group, out var classNetCachePath))
            {
                if (fullPath)
                {
                    classNetCachePath = $"{Utilities.RemoveAllPathPrefixes(group)}_ClassNetCache";
                }
                else
                {
                    classNetCachePath = $"{Utilities.RemoveAllPathPrefixes(group)}_ClassNetCache";
                }

                _cleanedClassNetCache[group] = classNetCachePath;
            }

            if (!NetFieldExportGroupMap.TryGetValue(classNetCachePath, out NetFieldExportGroup exportGroup))
            {
                return default;
            }

            return exportGroup;
        }
    }
}
