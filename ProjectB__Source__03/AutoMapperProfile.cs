using AutoMapper;
using ProjectB.Models.DTOS;
using ProjectB.Models.PrototypeModels;
using ProjectB.Models.PrototypeVersionModels;
using StageTest.Models.ContainerLineFolder;
using StageTest.Models.ContainerModels;
using StageTest.Models.ContainersVariablesModels;
using StageTest.Models.FolderModels;

namespace ProjectB
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //container
            CreateMap<Container, OutputContainerDto>();
            CreateMap<CreateContainer, Container>();
            //ContianerFolder
            CreateMap<ContainersFolder, OutputContainersFoldersDto>();
            CreateMap<CreateFolder, ContainersFolder>();
            //ContainersVariables
            CreateMap<ContainersVariable, CreateVariable>();
            CreateMap<CreateVariable, ContainersVariable>();
            //containerVariable 
            CreateMap<ContainersLine, CreateLine>();
            CreateMap<CreateLine, ContainersLine>();

            //Prototype
            CreateMap<Prototype, CreatePrototype>();
            CreateMap<CreatePrototype, Prototype>();

            //PrototypeServices
            CreateMap<PrototypesVersion, CreatePrototypeVersion>();
            CreateMap<CreatePrototypeVersion, PrototypesVersion>();
        }
    }
}
