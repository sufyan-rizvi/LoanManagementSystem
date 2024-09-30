using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappers
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<LoanScheme, LoanSchemeDTO>();
        }
    }
}