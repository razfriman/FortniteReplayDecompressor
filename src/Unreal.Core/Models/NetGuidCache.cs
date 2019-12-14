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
        public Dictionary<uint, string> NetGuidToPathName{ get; private set; } = new Dictionary<uint, string>();

        private Dictionary<uint, NetFieldExportGroup> _archTypeToExportGroup = new Dictionary<uint, NetFieldExportGroup>();
        public Dictionary<uint, NetFieldExportGroup> NetFieldExportGroupMapPathFixed { get; private set; } = new Dictionary<uint, NetFieldExportGroup>();
        private Dictionary<uint, string> _cleanedPaths = new Dictionary<uint, string>();

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
            NetFieldExportGroupMap[group] = exportGroup;
        }

        public NetFieldExportGroup GetNetFieldExportGroup(Actor actor, out string testPath)
        {
            var guid = actor.Archetype;
            var isActor = false;
            testPath = string.Empty;

            if(guid == null)
            {
                guid = actor.ActorNetGUID;
                isActor = true;
            }

            if (!_archTypeToExportGroup.ContainsKey(guid.Value))
            {
                var path = NetGuidToPathName[guid.Value];

                if(isActor)
                {
                    //The default types never end up here.
                    return null;

                    var tempPath = CoreRedirects.GetRedirect(path);

                    if (!String.IsNullOrEmpty(tempPath))
                    {
                        path = tempPath;
                    }
                    else
                    {
                        testPath = path;
                    }
                }

                if (NetFieldExportGroupMapPathFixed.ContainsKey(guid.Value))
                {
                    _archTypeToExportGroup[guid.Value] = NetFieldExportGroupMapPathFixed[guid.Value];

                    return NetFieldExportGroupMapPathFixed[guid.Value];
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
                        NetFieldExportGroupMapPathFixed[guid.Value] = NetFieldExportGroupMap[groupPath];
                        _archTypeToExportGroup[guid.Value] = NetFieldExportGroupMap[groupPath];

                        return NetFieldExportGroupMap[groupPath];
                    }
                }

                return null;
            }
            else
            {
                return _archTypeToExportGroup[guid.Value];
            }
        }

        public string RemoveAllPathPrefixes(string path)
        {
            path = RemovePathPrefix(path, "Default__");

            for(int i = path.Length - 1; i >= 0; i--)
            {
                switch(path[i])
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

        private string RemovePathSuffix(string path)
        {
            return Regex.Replace(path, @"(_?[0-9]+)+$", "");
        }

        private string RemovePathSuffix(string path, string toRemove)
        {
            return Regex.Replace(path, $@"{toRemove}$", "");
        }
    }
}
