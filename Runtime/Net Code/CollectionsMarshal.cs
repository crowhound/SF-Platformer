using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace System.Runtime.InteropServices
{
	public static class CollectionsMarshal
	{
		public static Span<T> AsSpan<T>(List<T> list)
		{
			if(list == null)
				return default(Span<T>);

			var box = new ListCastHelper { List = list }.StrongBox;
			return new Span<T>((T[])box.Value, 0, list.Count);
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct ListCastHelper
		{
			[FieldOffset(0)]
			public StrongBox<Array> StrongBox;

			[FieldOffset(0)]
			public object List;
		}
	}
}