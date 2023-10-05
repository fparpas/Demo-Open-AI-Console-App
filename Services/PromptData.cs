using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoOpenAI.Services
{
    // This class is used to read the System Prompts from the text files
    public static class PromptData
    {
        public static string SystemPrompt_EcommerceAssistant
        {
            get
            {
                return File.ReadAllText("./Data/SystemPrompt_EcommerceAssistant.txt");
            }
        }

        public static string SystemPrompt_EcommerceAssistant_Support
        {
            get
            {
                return File.ReadAllText("./Data/SystemPrompt_EcommerceSupportAssistant.txt");
            }
        }

        public static string SystemPrompt_AIAssistant
        {
            get
            {
                return File.ReadAllText("./Data/SystemPrompt_AIAssistant.txt");
            }
        }        
    }
}
