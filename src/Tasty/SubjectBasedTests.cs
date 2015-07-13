using System.Linq;

namespace Tasty
{
    public abstract class SubjectBasedTests<SubjectType> : UnitTests
    {
        protected SubjectType Subject { get; private set; }

        protected virtual SubjectType CreateSubject()
        {
            var construcorInfo = typeof(SubjectType).GetConstructors().Single();
            var parameterInfos = construcorInfo.GetParameters();
            object[] parameters = new object[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                parameters[i] = Mock.Of(parameterInfos[i].ParameterType);
            }
            return (SubjectType)construcorInfo.Invoke(parameters);
        }

        protected override void InitializeTest()
        {
            base.InitializeTest();
            Subject = CreateSubject();
        }
    }
}
