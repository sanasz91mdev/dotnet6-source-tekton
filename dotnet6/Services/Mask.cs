using DataAccess.EFCore.Mask;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class Mask
    {
        private IMemoryCache memoryCache;
        private IConfiguration config;

        public Mask(IMemoryCache cache, IConfiguration configuration)
        {
            memoryCache = cache;
            config = configuration;
        }

        public string[] getMaskedList()
        {
            string[] taglist;
            var output = memoryCache.Get<string[]>("blacklist");
            if (output == null)
            {
                var maskEntries = this.getMaskData();
                taglist = maskEntries.Select(x => x.Tag).ToArray();
                var invalidateCacheMinutes = config.GetSection("InvalidateCache").Get<int>();
                memoryCache.Set("blacklist", taglist, TimeSpan.FromMinutes(invalidateCacheMinutes));

            }
            else
            {
                taglist = output;
            }
            return taglist;
        }


        public List<MaskEntry> getMaskData()
        {
            List<MaskEntry> maskSettings = new List<MaskEntry>();
            var output = memoryCache.Get<List<MaskEntry>>("maskprofile");
            if (output == null)
            {
                //TODO: pass conn string from iconfig instance here in context ctor
                using (var maskContext = new MaskContext())
                {
                    Console.WriteLine(maskContext.Database.GetConnectionString());
                    var maskSetting = maskContext.MaskSettings.FirstOrDefault(x => x.SettingName == "SuiteProfile");
                    if (maskSetting == null) { return maskSettings; }

                    maskSettings = (from MSD in maskContext.Set<MaskSettingDetail>()
                                    join MF in maskContext.Set<MaskField>() on MSD.FieldId equals MF.FieldId
                                    where MSD.SettingId == maskSetting.SettingId
                                    select new MaskEntry
                                    {
                                        Tag = MF.Tags,
                                        StartIndex = Convert.ToInt32(MSD.StartIndex),
                                        Length = Convert.ToInt32(MSD.Length),
                                        MaskCharacter = MSD.MaskCharacter,
                                        MaskType = MSD.MaskType
                                    }).ToList();

                    foreach (var item in maskSettings)
                    {
                        Console.WriteLine(item.Length + " " + item.MaskCharacter + " " + item.Tag);
                    }
                }

                var invalidateCacheMinutes = config.GetSection("InvalidateCache").Get<int>();
                memoryCache.Set("maskprofile", maskSettings, TimeSpan.FromMinutes(invalidateCacheMinutes));

            }
            else
            {
                maskSettings = output;
            }
            return maskSettings;

        }
    }


    public class MaskEntry
    {
        public string Tag { get; set; }
        public string MaskType { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public string MaskCharacter { get; set; }

    }
}
