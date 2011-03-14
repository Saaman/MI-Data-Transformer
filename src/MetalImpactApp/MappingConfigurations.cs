using System;
using System.Globalization;
using System.Collections.Generic;
using AutoMapper;
using MIProgram.Model;
using Album = MIProgram.WorkingModel.Album;
using Artist = MIProgram.WorkingModel.Artist;
using Review = MIProgram.WorkingModel.Review;

namespace MetalImpactApp
{
    public static class MappingConfigurations
    {
        static readonly DateTimeFormatInfo CustomDateTimeFormatInfo = new DateTimeFormatInfo
                                         {
                                             LongDatePattern = "d MMMMM yyyy",
                                             MonthNames =
                                                 new[] { "janvier", "février", "mars", "avril", "mai", "juin", "juillet", "août", "septembre", "octobre", "novembre", "décembre", string.Empty }
                                         };
        static MappingConfigurations()
        {
            Mapper.CreateMap<MIProgram.WorkingModel.Reviewer, Reviewer>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CreationDate, x => x.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.Mail, x => x.MapFrom(src => src.MailAddress))
                .ForMember(dest => dest.Password, x => x.MapFrom(src => src.Name + src.UserId));

            Mapper.CreateMap<Artist, MIProgram.Model.Artist>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Id))
                .ForMember(dest => dest.ArtistName, x => x.MapFrom(src => src.Name))
                .ForMember(dest => dest.ArtistBiography, x => x.Ignore())
                .ForMember(dest => dest.ArtistCountries, x => x.Ignore())
                .ForMember(dest => dest.ArtistParsedCountries, x => x.Ignore())
                .ForMember(dest => dest.ArtistActivity, x => x.Ignore())
                .ForMember(dest => dest.ArtistLineUpMember, x => x.Ignore())
                .ForMember(dest => dest.ArtistSimilarArtists, x => x.Ignore())
                .ForMember(dest => dest.ArtistSimilarArtistsNames, x => x.MapFrom(src => src.SimilarArtists))
                .ForMember(dest => dest.ArtistLink, x => x.MapFrom(src => new Uri(src.OfficialUrl)))
                .ForMember(dest => dest.ArtistUnparsedCountries, x => x.MapFrom(src => src.OriginCountry))
                .ForMember(dest => dest.ArtistCreationDate, x => x.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.Reviewer, x => x.MapFrom(src => Mapper.Map<MIProgram.WorkingModel.Reviewer, Reviewer>(src.Reviewer)));

            Mapper.CreateMap<Album, MIProgram.Model.Album>()
                .ForMember(dest => dest.AlbumCommercialCode, x => x.Ignore())
                .ForMember(dest => dest.AlbumCover, x => x.MapFrom(src => new Image(src.CoverFileName)))
                .ForMember(dest => dest.AlbumDiscs, x => x.Ignore())
                .ForMember(dest => dest.AlbumLabel, x => x.MapFrom(src => src.Label))
                .ForMember(dest => dest.AlbumMainStyles, x => x.Ignore())
                .ForMember(dest => dest.AlbumMainStyle2, x => x.Ignore())
                .ForMember(dest => dest.AlbumStyleAlterations, x => x.Ignore())
                .ForMember(dest => dest.AlbumUnparsedStyle, x => x.MapFrom(src => src.MusicType))
                .ForMember(dest => dest.AlbumNumberOfDiscs, x => x.Ignore())
                .ForMember(dest => dest.AlbumOriginalReleaseDate, x => x.Ignore())
                .ForMember(dest => dest.AlbumReleaseDate, x => x.MapFrom(src => DateTime.Parse(src.ReleaseDate, CustomDateTimeFormatInfo)))
                .ForMember(dest => dest.AlbumTitle, x => x.MapFrom(src => src.Title))
                .ForMember(dest => dest.SharedArtist, x => x.MapFrom(src => Mapper.Map<Artist, MIProgram.Model.Artist>(src.Artist)))
                .ForMember(dest => dest.AlbumUnParsedType, x => x.MapFrom(src => src.RecordType))
                .ForMember(dest => dest.SharedScore, x => x.Ignore())
                .ForMember(dest => dest.AlbumParsedStyle, x => x.Ignore())
                .ForMember(dest => dest.AlbumParsedType, x => x.Ignore())
                .ForMember(dest => dest.SharedVisitorsScore, x => x.Ignore());

            Mapper.CreateMap<Album, Product>().ConvertUsing(Mapper.Map<Album, MIProgram.Model.Album>);

            Mapper.CreateMap<Review, MIProgram.Model.Review>()
                .ForMember(dest => dest.ReviewHits, x => x.MapFrom(src => src.Hits))
                .ForMember(dest => dest.ReviewNote, x => x.MapFrom(src => src.Score))
                .ForMember(dest => dest.ReviewText, x => x.MapFrom(src => src.Text))
                .ForMember(dest => dest.ReviewProduct, x => x.MapFrom(src => Mapper.Map<Album, Product>(src.Album)));
        }

        public static void Init()
        {
            Mapper.AssertConfigurationIsValid();
        }
    }

    public class UpperInvariantComparer : IEqualityComparer<string>
    {
        public int GetHashCode(string s)
        {
            return s.ToUpperInvariant().GetHashCode();
        }

        public bool Equals(string s1, string s2)
        {
            return s1.ToUpperInvariant() == s2.ToUpperInvariant();
        }
    }
}