﻿using CaseExtensions;
using Nexus.Extensions;

namespace Nexus.Models;

public class ServiceInitializationInfo
{
    public ServiceInitializationInfo(
        string solutionName,
        string serviceNameRaw,
        string basePath,
        int httpsPort,
        int httpPort,
        int dbPort)
    {
        SolutionName = solutionName;
        ServiceNameRaw = serviceNameRaw;
        BasePath = basePath;
        HttpsPort = httpsPort;
        HttpPort = httpPort;
        DbPort = dbPort;
    }
    
    public string ServiceToken { get; set; } = string.Empty;
    public string CertificatePassword { get; set; } = "dev123";
    public int HttpPort { get; set; }
    public int HttpsPort { get; set; }
    public string DbHost => $"{ServiceNameKebabCase}-db";

    public string DbName => $"{SolutionNameSnakeCase}_{ServiceNameKebabCase}";
    public int DbPort { get; set; }

    public string RootNamespace => ServiceNamePascalCasedAndDotApi.Replace(".", "");
    
    public string ServiceNameRaw { get; }
    public string ServiceNameKebabCase => NameExtensions.GetKebabCasedNameWithoutApi(ServiceNameRaw);
    public string ServiceNameKebabCaseAndApi => NameExtensions.GetKebabCasedNameAndApi(ServiceNameRaw);
    public string ServiceNameSnakeCase => ServiceNameRaw.ToSnakeCase();
    public string ServiceNameSnakeCaseAndApi => NameExtensions.GetSnakeCasedNameAndApi(ServiceNameRaw);
    public string ServiceNamePascalCasedAndDotApi => NameExtensions.GetPascalCasedNameAndDotApi(ServiceNameRaw);
    
    public string ServiceRootFolder => Path.Combine(BasePath, "services", ServiceNameKebabCaseAndApi);
    public string ServiceCsProjectFolder => Path.Combine(ServiceRootFolder, "src", ServiceNamePascalCasedAndDotApi);
    public string ServiceCsProjectFile => Path.Combine(ServiceCsProjectFolder, $"{ServiceNamePascalCasedAndDotApi}.csproj");
    
    
    public string SolutionName { get; } 
    public string SolutionNameSnakeCase => SolutionName.ToSnakeCase();

    public string BasePath { get; }
    public string SolutionPath => Path.Combine(BasePath, $"{SolutionName}.sln");
}