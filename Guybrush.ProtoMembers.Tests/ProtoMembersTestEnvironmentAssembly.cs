using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

[assembly: RequiresSTA]

[SetUpFixture]
public class ProtoMembersTestEnvironmentAssembly : ExtensionTestEnvironmentAssembly<ITestEnvironmentZone>
{
}

[ZoneDefinition]
public interface ITestEnvironmentZone : ITestsZone, IRequire<PsiFeatureTestZone>, IRequire<ILanguageCSharpZone>, IRequire<ICodeEditingZone>
{
}