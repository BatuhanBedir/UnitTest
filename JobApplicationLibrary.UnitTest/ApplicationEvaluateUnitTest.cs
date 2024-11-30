using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services;
using Moq;
using NUnit.Framework;

namespace JobApplicationLibrary.UnitTest;

public class ApplicationEvaluateUnitTest
{

    //UnitOfWork_Condition_ExceptedResult 
    //Func.UnderTest_ExceptedResult_UnderCondition
    [Test]
    public void Application_WithUnderAge_TransferredToAutoRejected()
    {
        //Arrage
        var evaluator = new ApplicationEvaluator(null);
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
        Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoRejected));
    }

    [Test]
    public void Application_WithNoTechStack_TransferredToAutoRejected()
    {
        //Arrage

        var mockValidator = new Mock<IIdentityValidator>(); //baðýmlýlýðý kaldýrdýrfake class oluþturuyor
        //mockValidator.Setup(x => x.IsValid("123")).Returns(true);// isValid x parametresiyle çalýþýrsa true dönsün.
        mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);//string olmasý yeterli
        //setup olmasa bile otomatik default deðerler dönecek.

        var evaluator = new ApplicationEvaluator(mockValidator.Object);
        var form = new JobApplication()
        {
            Applicant = new Applicant()
            {
                Age = 19
            },
            TechStackList = [""]
        };

        //Action
        var appResult = evaluator.Evaluate(form);

        //Assert
        Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoRejected));
    }

    [Test]
    public void Application_WithTechStackOver75P_TransferredToAutoAccepted()
    {
        //Arrage

        var mockValidator = new Mock<IIdentityValidator>();
        mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

        var evaluator = new ApplicationEvaluator(mockValidator.Object);
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
        Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoAccepted));
    }
    [Test]
    public void Application_WithInValidIdentityNumber_TransferredToHR()
    {
        //Arrage

        var mockValidator = new Mock<IIdentityValidator>();
        mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

        var evaluator = new ApplicationEvaluator(mockValidator.Object);
        var form = new JobApplication()
        {
            Applicant = new Applicant()
            {
                Age = 20
            }
        };

        //Action
        var appResult = evaluator.Evaluate(form);

        //Assert
        Assert.That(appResult, Is.EqualTo(ApplicationResult.TransferredToHR));
    }
}