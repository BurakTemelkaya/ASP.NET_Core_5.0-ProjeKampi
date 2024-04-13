using CoreLayer.Utilities.Results;

namespace CoreLayer.Utilities.Business
{
    public class BusinessRules
    {
        public static IResultObject Run(params IResultObject[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    return logic;
                }
            }

            return new SuccessResult();
        }
    }
}
