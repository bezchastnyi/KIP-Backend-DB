﻿using System;
using System.Text.RegularExpressions;
using AutoMapper;
using KIP_Backend.Extensions;
using KIP_Backend.Models.KIP.NoAuth;
using KIP_server_NoAuth.Models.KhPI;

namespace KIP_server_NoAuth.Mapping.Converters
{
    /// <summary>
    /// AudienceConverter.
    /// </summary>
    public class AudienceConverter : ITypeConverter<AudienceKhPI, Audience>
    {
        /// <summary>
        /// Convert model of audience from KhPI to KIP.
        /// </summary>
        /// <returns>Object of audience of model audience KIP.</returns>
        /// <param name="source">Model of audience KhPI.</param>
        /// <param name="destination">Model of audience KIP.</param>
        /// <param name="context">The context. </param>
        public Audience Convert(AudienceKhPI source, Audience destination, ResolutionContext context)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(source.title))
            {
                return null;
            }

            return new Audience
            {
                AudienceId = source.id,
                AudienceName = ConvertExtensions.FixTitle(source.title),
                NumberOfSeats = SearchNumberOfSeats(source.title),
            };
        }

        private static int? SearchNumberOfSeats(string title)
        {
            var regex = new Regex(@"[\d[0-9]{0,4} місць]");
            var matches = regex.Matches(title);
            if (matches.Count == 0)
            {
                return null;
            }

            foreach (Match match in matches)
            {
                regex = new Regex(@"\d[0-9]{0,4}");
                var matches2 = regex.Matches(match.Value);
                foreach (Match match2 in matches2)
                {
                    if (matches2.Count > 0)
                    {
                        return int.Parse(match2.Value);
                    }
                }
            }

            return null;
        }
    }
}
