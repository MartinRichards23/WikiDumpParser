using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDumpParser.Models
{
    public enum Namepaces
    {
        Media = -2,
        Special = -1,
        Main = 0,
        Talk = 1,
        User = 2,
        UserTalk = 3,
        Wikipedia = 4,
        WikipediaTalk = 5,
        File = 6,
        FileTalk = 7,
        MediaWiki = 8,
        MediaWikiTalk = 9,
        Template = 10,
        TemplateTalk = 11,
        Help = 12,
        HelpTalk = 13,
        Category = 14,
        CategoryTalk = 15,
        Portal = 100,
        PortalTalk = 101,
        Book = 108,
        BookTalk = 109,
        Draft = 118,
        DraftTalk = 119,
        EducationProgram = 446,
        EducationProgramTalk = 447,
        TimedText = 710,
        TimedTextTalk = 711,
        Module = 828,
        ModuleTalk = 829,
    };
}
