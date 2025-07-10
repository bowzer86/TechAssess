# Signal Booster Assignment

- What IDE or tools I used: 
    - Visual Studio 2022
- AI development tools:
    - GitHub Copilot
    - ChatGPT
    - Ollama
- Any assumptions, limitations, or future improvements
    - Assumptions:
        - .NET 9.0 vs 8.0
        - xUnit over NUnit or MSTest
        - Git Bash configured for checking out Windows-style line endings, commit Unix-style
        - Basic Console.WriteLine was adequate for Logging requirement in lieu of a more complex logging framework
        - Solution and project folder structure was flexible
        - Ollama was acceptable for proof of concept AI integration, as it is a free tool with no API key required
        - If AI parser throws an error during reading the note, it will resort to using the manual parsing method instead.
        - Default format is JSON, with plaintext as a fallback
    - Limitations:
        - The AI model may not always parse the input correctly due to its unpredictable nature.
        - The free AI model's temperature setting cannot be set to 0, which may lead to inconsistent results in unit tests.
        - I had limited access to AI code assist tools, since I was on the free tier of GitHub CoPilot.
    - Future Improvements:
        - Implement a more robust logging framework (e.g., Serilog, NLog) for better logging capabilities.
        - Add more comprehensive unit tests to cover edge cases and improve test reliability.
        - Consider using a more advanced AI model or service with better control over parameters like temperature.
        - Enhance the AI parser to handle more complex input formats and improve error handling.
        - Implement a configuration file for easier management of settings and parameters.
- Instructions to run the project
    - In order to use AI integration, you'll need to install Ollama from https://ollama.com/
    - After installing Ollama, run the following command to pull the llama3.1:8b model:
    ```ollama pull llama3.1:8b``` (this could take a few minutes to download the model)
    - You can modify the application settings in `src/TechAssess/appsettings.json`
    - Once that's done, you can Run or Debug directly from Visual Studio OR run in Terminal: 
    ```dotnet run --project src/TechAssess```
    - To Test: Run DmeOrderParserTests from Visual Studio or run ```dotnet test``` in Terminal (note that some Unit tests will fail intermittenly due to the unpredictable nature of AI's answers and inability to set the "temperature" setting to 0 on this particular model).