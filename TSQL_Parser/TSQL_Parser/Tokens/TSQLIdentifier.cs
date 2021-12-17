using System;

namespace TSQL.Tokens
{
	public class TSQLIdentifier : TSQLToken
	{
		internal protected TSQLIdentifier(
			int beginPosition,
			string text) :
			base(
				beginPosition,
				text)
		{
            if (Text?.Length > 0)
            {
                var first = Text[0];
                if (first == '[')
                {
                    Name = Text
                        .Substring(1, Text.Length - 2)
                        .Replace("]]", "]");
                }
                // check for quoted identifiers here just in case
                else if (first == '\"')
                {
                    Name = Text
                        .Substring(1, Text.Length - 2)
                        .Replace("\"\"", "\"");
                }
                else if (Text.Length >= 2 && first == 'N' && Text[1] == '\"')
                {
                    Name = Text
                        .Substring(2, Text.Length - 3)
                        .Replace("\"\"", "\"");
                }
                else
                    Name = Text;
            }
            else
                Name = Text;

            //if (Text.StartsWith("["))
            //{
            //    Name = Text
            //        .Substring(1, Text.Length - 2)
            //        .Replace("]]", "]");
            //}
            //// check for quoted identifiers here just in case
            //else if (Text.StartsWith("\""))
            //{
            //    Name = Text
            //        .Substring(1, Text.Length - 2)
            //        .Replace("\"\"", "\"");
            //}
            //else if (Text.StartsWith("N\""))
            //{
            //    Name = Text
            //        .Substring(2, Text.Length - 3)
            //        .Replace("\"\"", "\"");
            //}
            //else
            //{
            //    Name = Text;
            //}
        }



		public override TSQLTokenType Type
		{
			get
			{
				return TSQLTokenType.Identifier;
			}
		}



		public override bool IsComplete
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		///		Unescaped value for the name of the identifier.
		/// </summary>
		public string Name
		{
			get;
			private set;
		}
	}
}
