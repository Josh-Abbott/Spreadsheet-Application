// <copyright file="GlobalSuppressions.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<To keep in line with the instructions for formatting in the assignment.>", Scope = "member", Target = "~M:SpreadsheetEngine.Cell.#ctor(System.Int32,System.Int32)")]
[assembly: SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<This will no longer become an issue in future assignments.>", Scope = "member", Target = "~F:SpreadsheetEngine.Spreadsheet.columnCount")]
[assembly: SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<This will no longer become an issue in future assignments.>", Scope = "member", Target = "~F:SpreadsheetEngine.Spreadsheet.rowCount")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "<Causes issues with other functionality.>", Scope = "member", Target = "~F:SpreadsheetEngine.Cell.rowIndex")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "<Causes issues with other functionality.>", Scope = "member", Target = "~F:SpreadsheetEngine.Cell.columnIndex")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "<Causes issues with other functionality.>", Scope = "member", Target = "~F:SpreadsheetEngine.Cell.text")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "<Causes issues with other functionality.>", Scope = "member", Target = "~F:SpreadsheetEngine.Cell.value")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1018:Nullable type symbols should be spaced correctly", Justification = "<Is unable to be resolved without causing another issue.>", Scope = "type", Target = "~T:SpreadsheetEngine.Spreadsheet")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "<Is unable to be fixed without causing another issue.>", Scope = "type", Target = "~T:SpreadsheetEngine.TextEditAction")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "<Is unable to be fixed without causing another issue.>", Scope = "type", Target = "~T:SpreadsheetEngine.ConstantNode")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1011:Closing square brackets should be spaced correctly", Justification = "<Is unable to be fixed without causing another issue.>", Scope = "type", Target = "~T:SpreadsheetEngine.Spreadsheet")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "<Is unable to be fixed without causing another issue.>", Scope = "type", Target = "~T:SpreadsheetEngine.BGColorEditAction")]
