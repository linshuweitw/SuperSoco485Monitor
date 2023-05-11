using CommandLine;

/// <summary>
/// Class containing the command line options
/// </summary>
public class CmdOptions
{
    [Value(0, MetaName = "InputFile", HelpText = "File to parse")]
    public String? InputFile { get; set; }
}