using Arohan.TMS.Application.Interfaces;
using Arohan.TMS.Infrastructure.Persistence;
using Arohan.TMS.Infrastructure.Repositories;
using Arohan.TMS.Infrastructure.Secrets;
using Arohan.TMS.Infrastructure.Tenancy;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// TenantProvider - you must implement a concrete TenantProvider that reads from current context (headers, claims, domain, etc.)
builder.Services.AddScoped<ITenantProvider, TenantProvider>(); // <-- implement TenantProvider

// Secret store
builder.Services.AddScoped<ISecretStore, VaultSecretStore>();

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

// MediatR & AutoMapper & Validators
builder.Services.AddMediatR(Assembly.GetExecutingAssembly()); // adjust assembly where handlers live
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); // FluentValidation

// Pipeline behaviors (validation, logging) - implement behaviors in Application layer
// builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

// IHttpContextAccessor (if not already registered)
builder.Services.AddHttpContextAccessor();

// Tenant provider (you already added earlier)
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

// Current user service
builder.Services.AddScoped<ICurrentUserService, Arohan.TMS.Infrastructure.Auth.CurrentUserService>();

// Secret store (existing)
builder.Services.AddScoped<ISecretStore, VaultSecretStore>();

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

// after app.UseRouting(); and before app.MapControllers() or app.MapBlazorHub()
app.UseMiddleware<Arohan.TMS.Web.Middleware.TenantMiddleware>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();
