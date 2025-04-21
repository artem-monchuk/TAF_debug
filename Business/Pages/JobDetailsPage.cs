using OpenQA.Selenium;
using Framework.Core.Pages;
using Framework.Core.Utilities;

namespace Framework.Business.Pages
{
    public class JobDetailsPage(IWebDriver driver) : BasePage(driver)
    {
        private readonly By _jobDescription = By.XPath("//div[contains(@class, 'vacancy-description')] | //div[contains(@class, 'description')]");

        public bool ValidateJobDescription(string keyword)
        {
            var descriptionElement = WaitAndFindElement(_jobDescription);
            var descriptionText = descriptionElement.Text.ToLower();
            var keywordExists = descriptionText.Contains(keyword.ToLower());

            if (keywordExists)
                Logger.LogInfo($"Successfully validated keyword '{keyword}' in job description");
            else
                Logger.LogError($"Keyword '{keyword}' not found in job description");

            return keywordExists;
        }
    }
}
