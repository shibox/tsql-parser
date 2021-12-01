using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSQL.Tokens
{
	public class TSQLSystemIdentifier : TSQLIdentifier
	{
		internal TSQLSystemIdentifier(
			int beginPosition,
			string text) :
			base(
				beginPosition,
				text)
		{
			Identifier = TSQLIdentifiers.Parse(text);
		}



		public override TSQLTokenType Type
		{
			get
			{
				return TSQLTokenType.SystemIdentifier;
			}
		}



		public TSQLIdentifiers Identifier
		{
			get;
			private set;
		}
	}
}
