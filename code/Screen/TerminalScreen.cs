#nullable enable

using Sandbox.Rendering;

public struct TerminalPos
{
	public int Row;
	public int Column;

	public TerminalPos( int row, int column )
	{
		Row = row;
		Column = column;
	}

}

/// <summary>
/// A set of characters designed to display on a terminal screen.
/// </summary>
public class TerminalScreen
{

	private char[] Contents;

	public int Width { get; private set; }

	public int Height { get; private set; }

	public TerminalScreen( int width, int height )
	{
		Contents = new char[width * height];
		this.Width = width;
		this.Height = height;
	}

	public TerminalScreen() : this( 132, 24 ) { }

	public TerminalScreen( TerminalScreen other )
	{
		this.Width = other.Width;
		this.Height = other.Height;
		this.Contents = (char[])other.Contents.Clone();
	}

	public char CharAt( int row, int column )
	{
		return Contents[row * Width + column];
	}

	public char CharAt( ref TerminalPos pos )
	{
		return CharAt( pos.Row, pos.Column );
	}

	public void SetCharAt( int row, int column, char c )
	{
		Contents[row * Width + column] = c;
	}

	public void SetCharAt( ref TerminalPos pos, char c )
	{
		SetCharAt( pos.Row, pos.Column, c );
	}

	public void Fill( char c )
	{
		Array.Fill( Contents, c );
	}

	public string GetRow( int index )
	{
		return new string( Contents, index * Width, Width );
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
	public TerminalPos WriteString( string str, int startRow, int startColumn = 0, bool wrap = false )
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
					return new TerminalPos(row, col);
				}
			}

			Contents[row * Width + col] = c;
			col++;
			count++;
		}

		return new TerminalPos(row, col);
	}
}
