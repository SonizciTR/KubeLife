﻿using AutoMapper;
using KubeLife.Domain.Models;
using KubeLife.Kubernetes.Models;

namespace KubeLife.Domain
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KubeCronJobModel, KubeCronJobModelView>();
            CreateMap<KubeRouteModel, KubeRouteViewModel>();
            //CreateMap<KubeJobModel, KubeJobViewModel>();
        }
    }
}
