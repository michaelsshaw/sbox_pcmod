public class AnsiTerminal
{
	private TerminalScreen ts;

	ushort w;
	ushort h;

	ushort cx;
	ushort cy;

	byte col;

	bool esc;

	public AnsiTerminal( TerminalScreen ts )
	{
		this.ts = ts;

		this.w = (ushort)ts.Width;
		this.h = (ushort)ts.Height;

		this.cx = 0;
		this.cy = 0;

		this.col = 0;
	}

	void Scroll()
	{
		for ( ushort y = 0; y < h - 1; y++ )
		{
			for ( ushort x = 0; x < w; x++ )
			{
				TerminalChar c = ts.CharAt( y + 1, x );
				ts.SetCharAt( y, x, c );
			}
		}

		for ( ushort x = 0; x < w; x++ )
			ts.SetCharAt( h - 1, x, new TerminalChar( 0, 0 ) );
	}

	void LineFeed()
	{
		cx = 0;
		if ( cy < h - 1 )
			cy++;
		else
			Scroll();
	}

	void Bell()
	{
	}

	void CursorStep()
	{
		if ( cx < w - 1 )
			cx++;
		else
			LineFeed();
	}

	public void PutChar( byte c )
	{
		TerminalChar tc = ts.Contents[cy * w + cx];
		switch ( (byte)c )
		{
			case (byte)'\x1b':
				esc = true;
				break;
			case (byte)'\n':
				LineFeed();
				return;
			case (byte)'\r':
				cx = 0;
				return;
			case (byte)'\t':
				for ( int i = 0; i < 8; i++ )
				{
					PutChar( (byte)' ' );
				}
				return;
			case (byte)'\b':
				tc.Character = 0;
				tc.Color = 0;
				if ( cx > 0 )
					cx--;
				return;
			case (byte)'\a':
				Bell();
				return;
			default:
				tc.Character = c;
				tc.Color = col;
				CursorStep();
				break;
		}
	}
}
