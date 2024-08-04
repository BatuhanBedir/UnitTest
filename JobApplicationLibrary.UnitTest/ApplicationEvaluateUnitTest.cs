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
}