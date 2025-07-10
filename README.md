# Signal Booster Assignment

- What IDE or tools I used: 
    - Visual Studio 2022
- AI development tools:
    - GitHub Copilot
    - Ollama (for AI model integration)
- Any assumptions, limitations, or future improvements
    - Assumed .NET 9.0 vs 8.0
    - Assumed xUnit over NUnit or MSTest
    - Assumed Git Bash configured for checking out Windows-style line endings, commit Unix-style
    - Assumed basic Console.WriteLine was adequate for Logging requirement in lieu of a more complex logging framework
    - Assumed solution and project folder structure was flexible
    - Assumed Ollama was acceptable for AI integration, as it is a free tool with no API key required
    - Assumed if the AI parser throws an error during reading the note, it will resort to using the manual parsing method instead.
    - Assumed default format is JSON, with plaintext as a fallback
- Instructions to run the project (if needed)
    - In order to use AI integration, you'll need to install Ollama from https://ollama.com/
    - After installing Ollama, run the following command to pull the llama3.1:8b model:
    ```ollama run llama3.1:8b``` (this could take a few minutes to download the model)
    - Once tha's done, you can Run or Debug directly from Visual Studio OR run in Terminal: 
    ```dotnet run --project src/TechAssess```
    - To Test: Run DmeOrderParserTests from Visual Studio or run ```dotnet test``` in Terminal (note that some Unit tests will fail intermittenly due to the unpredictable nature of AI's answers and inability to set the "temperature" setting to 0 on this particular model).