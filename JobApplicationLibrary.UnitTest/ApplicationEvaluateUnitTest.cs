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

        var mockValidator = new Mock<IIdentityValidator>(); //ba��ml�l��� kald�rd�rfake class olu�turuyor
        //mockValidator.Setup(x => x.IsValid("123")).Returns(true);// isValid x parametresiyle �al���rsa true d�ns�n.
        mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);//string olmas� yeterli
        //setup olmasa bile otomatik default de�erler d�necek.

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
        var mockValidator = new Mock<IIdentityValidator>(MockBehavior.Loose);
        //var mockValidator = new Mock<IIdentityValidator>(MockBehavior.Strict);
        mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
       // mockValidator.Setup(x => x.CheckConnectionToRemoteServer()).Returns(false);

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

#region MockBehavior.Strict
/*
    Sadece a��k�a ayarlanm�� (setup edilmi�) olan metotlar veya �zellikler �a�r�labilir.
    Testin s�k� kontrol edilmesi gereken durumlarda

    Kullan�lan b�t�n metotlar�n setup yazmak gerekiyor.
    daha k�r�lgan testler
 */
#endregion

#region MockBehavior.Loose
/*
    Ayarlanmayan metotlar ve �zellikler �a�r�labilir, ancak varsay�lan de�er d�nd�r�rler.
    S�k� denetim gerektirmedi�i, sadece belirli metotlar�n veya �zelliklerin test edilmek istendi�i durumlarda

    return default value
 */
#endregion