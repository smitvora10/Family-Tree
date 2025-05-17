using FamilyTree.BL.Services;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;

namespace FamilyTree.Extensions
{
    public static class AllServiceCollection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(DBCommon<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BLCommon<>));


            //services.AddTransient<IBaseRepository<RelationType>, DBCommon<RelationType>>();
            services.AddScoped<IRelationTypeRepository, DBRelationType>();
            services.AddScoped<IRelationTypeService, BLRelationType>();

            services.AddScoped<IPersonRepository, DBPerson>();
            services.AddScoped<IPersonService, BLPerson>();

            services.AddScoped<IOccupationRepository, DBOccupation>();
            services.AddScoped<IOccupationService, BLOccupation>();

            services.AddScoped<IQualificationRepository, DBQualification>();
            services.AddScoped<IQualificationService, BLQualification>();   
            
            services.AddScoped<IAuthRepository, DBAuth>();
            services.AddScoped<IAuthService, BLAuth>();

            services.AddScoped<IRequestRepository, DBRequest>();
            services.AddScoped<IRequestService, BLRequest>();
              
            services.AddScoped<IUserRepository, DBUser>();
            services.AddScoped<IUserService, BLUser>();

        }
    }
}
