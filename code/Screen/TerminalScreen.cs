#nullable enable

using Sandbox.Rendering;

/// <summary>
/// A character that can be displayed in the terminal.
/// </summary>
public struct TerminalChar
{
	public TerminalChar(byte character, byte color)
	{
		this.Character = character;
		this.Color = color;
	}

	/// <summary>
	/// The ASCII code of the character to write.
	/// </summary>
	public byte Character;

	/// <summary>
	/// The 4-bit colors of the character & background
	/// </summary>
	public byte Color;
}

/// <summary>
/// A set of characters designed to display on a terminal screen.
/// </summary>
public class TerminalScreen
{

	public TerminalChar[] Contents { get; }

	public int Width { get; private set; }

	public int Height { get; private set; }

	public TerminalScreen( int width, int height )
	{
		Contents = new TerminalChar[width * height];
		this.Width = width;
		this.Height = height;
	}

	public TerminalScreen() : this( 80, 24 ) { }

	public TerminalScreen( TerminalScreen other )
	{
		this.Width = other.Width;
		this.Height = other.Height;
		this.Contents = (TerminalChar[])other.Contents.Clone();
	}

	public TerminalChar CharAt( int row, int column )
	{
		return Contents[row * Width + column];
	}

	public void SetCharAt( int row, int column, TerminalChar c )
	{
		Contents[row * Width + column] = c;
	}

	public void SetCharAt(int row, int column, byte c, byte color = 0)
	{
		Contents[row * Width + column] = new TerminalChar(c, color);
	}

	public void Fill( TerminalChar c )
	{
		Array.Fill( Contents, c );
	}

	/// <summary>
	/// Write a string to the terminal.
	/// </summary>
	/// <param name="str">String to write.</param>
	/// <param name="startRow">Row index to start on.</param>
	/// <param name="startColumn">Column index to start on.</param>
	/// <param name="wrap">If true, string will wrap to the next line. 
	/// Otherwise, it will be cut off at the end of the row.</param>
	/// <returns>The location of the "cursor" after the string is written.</returns>
	public void WriteString( string str, ushort startRow, ushort startColumn = 0, bool wrap = false )
	{
		int row = startRow;
		int col = startColumn;

		int count = 0;
		foreach ( char c in str )
		{
			// Go to the next row if wrap
			if ( col >= Width )
			{
				if ( wrap )
				{
					col = 0;
					row++;
				}
				else
				{
				}
			}

			Contents[row * Width + col] = new TerminalChar((byte)c, 0);
			col++;
			count++;
		}

	}
}