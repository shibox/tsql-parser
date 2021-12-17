using System;

namespace TSQL.Tokens
{
	public class TSQLKeyword : TSQLToken
	{
		internal TSQLKeyword(
			int beginPosition,
			string text) :
			base(
				beginPosition,
				text)
		{
			Keyword = TSQLKeywords.Parse(text);
		}

        internal TSQLKeyword(
            int beginPosition,
            TSQLKeywords keyword) :
            base(
                beginPosition,
                keyword.Keyword)
        {
            Keyword = keyword;
        }

        public override TSQLTokenType Type
		{
			get
			{
				return TSQLTokenType.Keyword;
			}
		}



		public TSQLKeywords Keyword
		{
			get;
			private set;
		}

		public override bool IsComplete
		{
			get
			{
				return true;
			}
		}
	}
}
