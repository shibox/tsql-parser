using System;
using System.Globalization;

namespace TSQL.Tokens
{
	public class TSQLNumericLiteral : TSQLLiteral
	{
		internal TSQLNumericLiteral(
			int beginPosition,
			string text) :
			base(
				beginPosition,
				text)
		{
            // have to handle exponent notation, e.g. 0.5E-2.
            // https://docs.microsoft.com/en-us/sql/t-sql/data-types/decimal-and-numeric-transact-sql
            // can be up to max size of decimal, which is 38 places of precision.

            //该语句严重影响解析速度，
            //Value = Double.Parse(Text, NumberStyles.Any, new CultureInfo("en-US"));
            Value = Double.Parse(Text);
        }



		public override TSQLTokenType Type
		{
			get
			{
				return TSQLTokenType.NumericLiteral;
			}
		}



		public double Value
		{
			get;

			private set;
		}
	}
}
