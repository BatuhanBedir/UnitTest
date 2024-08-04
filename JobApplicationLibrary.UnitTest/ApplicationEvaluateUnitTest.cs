using JobApplicationLibrary.Models;

namespace JobApplicationLibrary.UnitTest;

public class ApplicationEvaluateUnitTest
{

    //UnitOfWork_Condition_ExceptedResult 
    //Func.UnderTest_ExceptedResult_UnderCondition
    [Test]
    public void Application_WithUnderAge_TransferredToAutoRejected()
    {
        //Arrage
        var evaluator = new ApplicationEvaluator();
        var form = new JobApplication()
        {
            Applicant = new Applicant()
            {
                Age = 17
            }
        };

        //Action
        var appResult = evaluator.Evaluate(form);

        //Assert
        Assert.AreEqual(appResult, ApplicationResult.AutoRejected);
    }

    [Test]
    public void Application_WithNoTechStack_TransferredToAutoRejected()
    {
        //Arrage
        var evaluator = new ApplicationEvaluator();
        var form = new JobApplication()
        {
            Applicant = new Applicant()
            {
                Age = 19
            },
            TechStackList = new List<string> { "" }
        };

        //Action
        var appResult = evaluator.Evaluate(form);

        //Assert
        Assert.AreEqual(appResult, ApplicationResult.AutoRejected);
    }

    [Test]
    public void Application_WithTechStackOver75P_TransferredToAutoAccepted()
    {
        //Arrage
        var evaluator = new ApplicationEvaluator();
        var form = new JobApplication()
        {
            Applicant = new Applicant()
            {
                Age = 20
            },
            TechStackList = new List<string> { "C#", "RabbitMQ", "Microservice", "Visual Studio" },
            YearsOfExperience = 16
        };

        //Action
        var appResult = evaluator.Evaluate(form);

        //Assert
        Assert.AreEqual(appResult, ApplicationResult.AutoAccepted);
    }
}