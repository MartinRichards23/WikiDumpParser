using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDumpParser.Models
{
    public class Page
    {
        public Page(int id, int namespaceKey, string redirect, string title, string text)
        {
            Id = id;
            Namespace = namespaceKey;
            Redirect = redirect;
            Title = title;
            Text = text;
        }

        public int Id { get; }
        public int Namespace { get; }
        public string Redirect { get; }
        public string Title { get; }
        public string Text { get; }

        public bool IsRedirect
        {
            get { return Redirect != null; }
        }
    }
}
