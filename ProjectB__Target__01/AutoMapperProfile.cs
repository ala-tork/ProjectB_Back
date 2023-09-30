using AutoMapper;
using ProjectB__Target__01.Models.ProjectModels;
using ProjectB__Target__01.Models.ProjectVersionModels;

namespace ProjectB__Target__01
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<ProjectsVersion, CreateProjectVersion>();
            CreateMap<CreateProjectVersion,ProjectsVersion>();

            //
            CreateMap<Project, CreateProject>();
            CreateMap<CreateProject, Project>();

        }
    }
}
