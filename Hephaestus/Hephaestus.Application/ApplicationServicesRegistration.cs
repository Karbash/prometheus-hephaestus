using FluentValidation;
using Hephaestus.Domain.DTOs.Request;
using Hephaestus.Application.Interfaces.Additional;
using Hephaestus.Application.Interfaces.Administration;
using Hephaestus.Application.Interfaces.Auth;
using Hephaestus.Application.Interfaces.Category;
using Hephaestus.Application.Interfaces.Company;
using Hephaestus.Application.Interfaces.Coupon;
using Hephaestus.Application.Interfaces.Customer;
using Hephaestus.Application.Interfaces.Database;
using Hephaestus.Application.Interfaces.Menu;
using Hephaestus.Application.Interfaces.OpenAI;
using Hephaestus.Application.Interfaces.Order;
using Hephaestus.Application.Interfaces.Payment;
using Hephaestus.Application.Interfaces.Promotion;
using Hephaestus.Application.Interfaces.Tag;
using Hephaestus.Application.Interfaces.WhatsApp;
using Hephaestus.Application.Services;
using Hephaestus.Application.UseCases;
using Hephaestus.Application.UseCases.Additional;
using Hephaestus.Application.UseCases.Administration;
using Hephaestus.Application.UseCases.Auth;
using Hephaestus.Application.UseCases.Category;
using Hephaestus.Application.UseCases.Company;
using Hephaestus.Application.UseCases.Coupon;
using Hephaestus.Application.UseCases.Customer;
using Hephaestus.Application.UseCases.Database;
using Hephaestus.Application.UseCases.Menu;
using Hephaestus.Application.UseCases.OpenAI;
using Hephaestus.Application.UseCases.Order;
using Hephaestus.Application.UseCases.Promotion;
using Hephaestus.Application.UseCases.Tag;
using Hephaestus.Application.UseCases.Review;
using Hephaestus.Application.UseCases.WhatsApp;
using Hephaestus.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Hephaestus.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        AddUseCases(services);
        AddValidators(services);
        AddServices(services);
        return services;
    }

    private static void AddUseCases(IServiceCollection services)
    {
        // Auth UseCases
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IRegisterCompanyUseCase, RegisterCompanyUseCase>();
        services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();
        services.AddScoped<IMfaUseCase, MfaUseCase>();

        // Administration UseCases
        services.AddScoped<IGetCompaniesUseCase, GetCompaniesUseCase>();
        services.AddScoped<IUpdateCompanyUseCase, UpdateCompanyUseCase>();
        services.AddScoped<ISalesReportUseCase, SalesReportUseCase>();
        services.AddScoped<IAuditLogUseCase, AuditLogUseCase>();
        services.AddScoped<IGetCompaniesWithinRadiusUseCase, GetCompaniesWithinRadiusUseCase>();

        // Customer UseCases
        services.AddScoped<IUpdateCustomerUseCase, UpdateCustomerUseCase>();
        services.AddScoped<IGetCustomerUseCase, GetCustomerUseCase>();
        services.AddScoped<IGetByIdCustomerUseCase, GetByIdCustomerUseCase>();

        // Menu UseCases
        services.AddScoped<ICreateMenuItemUseCase, CreateMenuItemUseCase>();
        services.AddScoped<IGetMenuItemsUseCase, GetMenuItemsUseCase>();
        services.AddScoped<IGetMenuItemByIdUseCase, GetMenuItemByIdUseCase>();
        services.AddScoped<IUpdateMenuItemUseCase, UpdateMenuItemUseCase>();
        services.AddScoped<IDeleteMenuItemUseCase, DeleteMenuItemUseCase>();

        // Category UseCases
        services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
        services.AddScoped<IGetCategoriesUseCase, GetCategoriesUseCase>();
        services.AddScoped<IGetCategoryByIdUseCase, GetCategoryByIdUseCase>();
        services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
        services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();

        // Tag UseCases
        services.AddScoped<ICreateTagUseCase, CreateTagUseCase>();
        services.AddScoped<IGetAllTagsByTenantUseCase, Hephaestus.Application.UseCases.Tags.GetAllTagsByTenantUseCase>();
        services.AddScoped<IDeleteTagUseCase, DeleteTagUseCase>();

        // WhatsApp UseCases
        services.AddScoped<IProcessWhatsAppMessageUseCase, ProcessWhatsAppMessageUseCase>();
        services.AddScoped<IIntentClassifierUseCase, IntentClassifierUseCase>();
        services.AddScoped<IActionPipelineUseCase, ActionPipelineUseCase>();

        // Promotion UseCases
        services.AddScoped<ICreatePromotionUseCase, CreatePromotionUseCase>();
        services.AddScoped<IGetPromotionsUseCase, GetPromotionsUseCase>();
        services.AddScoped<IGetPromotionByIdUseCase, GetPromotionByIdUseCase>();
        services.AddScoped<IUpdatePromotionUseCase, UpdatePromotionUseCase>();
        services.AddScoped<IDeletePromotionUseCase, DeletePromotionUseCase>();
        services.AddScoped<INotifyPromotionUseCase, NotifyPromotionUseCase>();

        // Additional UseCases
        services.AddScoped<ICreateAdditionalUseCase, CreateAdditionalUseCase>();
        services.AddScoped<IGetAdditionalsUseCase, GetAdditionalsUseCase>();
        services.AddScoped<IGetAdditionalByIdUseCase, GetAdditionalByIdUseCase>();
        services.AddScoped<IUpdateAdditionalUseCase, UpdateAdditionalUseCase>();
        services.AddScoped<IDeleteAdditionalUseCase, DeleteAdditionalUseCase>();

        // Company UseCases
        services.AddScoped<IGetCompanyProfileUseCase, GetCompanyProfileUseCase>();

        // OpenAI
        services.AddHttpClient<IChatWithOpenAIUseCase, ChatWithOpenAIUseCase>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(60);
            client.DefaultRequestHeaders.Add("User-Agent", "Hephaestus-API/1.0");
        });
        services.AddScoped<IChatWithOpenAIUseCase, ChatWithOpenAIUseCase>();

        // Database
        services.AddScoped<IExecuteQueryUseCase, ExecuteQueryUseCase>();

        //Coupon
        services.AddScoped<ICreateCouponUseCase, CreateCouponUseCase>();
        services.AddScoped<IGetCouponsUseCase, GetCouponsUseCase>();
        services.AddScoped<IGetCouponByIdUseCase, GetCouponByIdUseCase>();
        services.AddScoped<IUpdateCouponUseCase, UpdateCouponUseCase>();
        services.AddScoped<IDeleteCouponUseCase, DeleteCouponUseCase>();

        //Order
        services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
        services.AddScoped<IGetOrdersUseCase, GetOrdersUseCase>();
        services.AddScoped<IGetOrderByIdUseCase, GetOrderByIdUseCase>();
        services.AddScoped<IUpdateOrderUseCase, UpdateOrderUseCase>();
        services.AddScoped<IGetCustomerOrderStatusUseCase, GetCustomerOrderStatusUseCase>();
        services.AddScoped<IPatchOrderUseCase, PatchOrderUseCase>();
        
        // Global Admin UseCases
        services.AddScoped<Hephaestus.Application.Interfaces.Order.IGlobalOrderAdminUseCase, GlobalOrderAdminUseCase>();
        services.AddScoped<Hephaestus.Application.Interfaces.Administration.IGlobalCategoryAdminUseCase, GlobalCategoryAdminUseCase>();
        services.AddScoped<IGetAllPromotionsAdminUseCase, GetAllPromotionsAdminUseCase>();
        services.AddScoped<IGlobalTagAdminUseCase, GlobalTagAdminUseCase>();
        services.AddScoped<IGlobalMenuItemAdminUseCase, GlobalMenuItemAdminUseCase>();
        services.AddScoped<IGlobalCouponAdminUseCase, GlobalCouponAdminUseCase>();

        //Payment
        services.AddScoped<IProcessPaymentUseCase, ProcessPaymentUseCase>();
        services.AddScoped<CreateReviewUseCase>();
    }

    private static void AddServices(IServiceCollection services)
    {
        // Exception Handling
        services.AddSingleton<IExceptionHandlerService, ExceptionHandlerService>();
        services.AddScoped<IConversationContextService, ConversationContextService>();
    }

    private static void AddValidators(IServiceCollection services)
    {
        // Menu Validators
        services.AddScoped<IValidator<CreateMenuItemRequest>, CreateMenuItemRequestValidator>();
        services.AddScoped<IValidator<UpdateMenuItemRequest>, UpdateMenuItemRequestValidator>();

        // Category Validators
        services.AddScoped<IValidator<CreateCategoryRequest>, CreateCategoryRequestValidator>();
        services.AddScoped<IValidator<UpdateCategoryRequest>, UpdateCategoryRequestValidator>();

        // Auth Validators
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
        services.AddScoped<IValidator<MfaRequest>, MfaRequestValidator>();
        services.AddScoped<IValidator<RegisterCompanyRequest>, RegisterCompanyRequestValidator>();
        services.AddScoped<IValidator<ResetPasswordConfirmRequest>, ResetPasswordConfirmRequestValidator>();
        services.AddScoped<IValidator<ResetPasswordRequest>, ResetPasswordRequestValidator>();

        // Company Validators
        services.AddScoped<IValidator<UpdateCompanyRequest>, UpdateCompanyRequestValidator>();

        // Customer Validators
        services.AddScoped<IValidator<CustomerRequest>, CustomerRequestValidator>();

        // OpenAI Validators
        services.AddScoped<IValidator<OpenAIRequest>, OpenAIChatRequestValidator>();

        // Database Validators
        services.AddScoped<IValidator<ExecuteQueryRequest>, ExecuteQueryRequestValidator>();

        // Additional Validators
        services.AddScoped<IValidator<CreateAdditionalRequest>, CreateAdditionalRequestValidator>();
        services.AddScoped<IValidator<UpdateAdditionalRequest>, UpdateAdditionalRequestValidator>();

        // Promotion Validators
        services.AddScoped<IValidator<CreatePromotionRequest>, CreatePromotionRequestValidator>();
        services.AddScoped<IValidator<UpdatePromotionRequest>, UpdatePromotionRequestValidator>();
        services.AddScoped<IValidator<NotifyPromotionRequest>, NotifyPromotionRequestValidator>();

        //Coupon Validators
        services.AddValidatorsFromAssemblyContaining<CreateCouponRequestValidator>();

        //Order Validators
        services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateOrderRequestValidator>();
    }
}
