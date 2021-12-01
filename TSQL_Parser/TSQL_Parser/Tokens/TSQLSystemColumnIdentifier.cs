using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSQL.Tokens
{
	public class TSQLSystemColumnIdentifier : TSQLIdentifier
	{
		internal TSQLSystemColumnIdentifier(
			int beginPosition,
			string text) :
			base(
				beginPosition,
				text)
		{
			
		}



		public override TSQLTokenType Type
		{
			get
			{
				return TSQLTokenType.SystemColumnIdentifier;
			}
		}


	}
}
