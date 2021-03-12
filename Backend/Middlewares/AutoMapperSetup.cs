﻿using System;
using AutoMapper;
using Backend.Extensions;

namespace Backend.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoMapperSetup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IConfigurationProvider SetupMapping()
        {
            return new MapperConfiguration(opts =>
            {
                opts.CreateMap<string, string>().ConvertUsing<NullStringConverter>();
                opts.CreateMap<DateTime, string>().ConvertUsing(dt => dt.ToString("dd/MM/yyyy HH:mm:ss"));
            });
        }
    }
}