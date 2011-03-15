using System;
using System.Collections.Generic;
using MIProgram.Model;

namespace MIProgram.Core.ProductStores
{
    public interface IProductRepository<T> where T: Product
    {
        Artist GetOrBuildArtist(string artistName, IList<Country> countries, string officialUrl, DateTime lastUpdate, Reviewer reviewer, IList<Artist> similarArtists);
        Reviewer GetOrBuildReviewer(string name, string mailAddress, DateTime lastUpdate);
        
        IList<Artist> Artists { get; }
        IList<Reviewer> Reviewers { get; }
        IList<T> Products { get; }
    }
}