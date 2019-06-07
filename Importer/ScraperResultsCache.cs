using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2.Import
{
    class ScraperResultsCache
    {
        string searchTerm;
        ScraperSearchParams searchParams;
        Dictionary<Scraper, ScraperResult> cachedResults;
        public ScraperResultsCache(string searchTerm, ScraperSearchParams searchParams)
        {
            this.searchTerm = ScraperProvider.RemoveSpecialChars(searchTerm);
            this.searchParams = searchParams;
            this.searchParams.Term = this.searchTerm; //update search term to matched title
            cachedResults = new Dictionary<Scraper, ScraperResult>();
        }

        public void Add(Scraper scraper, ScraperResult result)
        {
            cachedResults[scraper] = result;
        }

        public ScraperResult GetResult(Scraper scraper)
        {
            ScraperResult result;
            if (!cachedResults.TryGetValue(scraper, out result))
            {
                result = scraper.GetFirstMatch(searchParams);
                if (result != null && FuzzyStringComparer.Score(searchTerm, ScraperProvider.RemoveSpecialChars(result.Title)) > 2)
                    result = null;
                cachedResults[scraper] = result;
            }
            return result;
        }
    }
}
