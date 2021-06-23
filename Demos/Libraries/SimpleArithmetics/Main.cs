/*
 * Created by SharpDevelop.
 * User: kormilicin
 * Date: 05.05.2021
 * Time: 15:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Lib;
using System;
using static Lib.CommonLibraryCore;

namespace SimpleArithmetics
{
    /// <summary>
    /// Description of Main.
    /// </summary>
    public class Main : CommonLibrary
	{
		public new string Name = "SimpleArithmetics";

		[Image("adds float a, float b, ... : double")]
		[@Description("Calculating a+b+...")]
		public double add()
		{
			var arguments = this.getArgumentsOfType<float>();
			double sum = 0;
			for (int i = 0; i < arguments.Length; i++)
				sum += Convert.ToDouble(arguments[i]);
			return sum;
		}

		[Image("subs float a, float b, float c, ... : double")]
		[@Description("Equivalent a-b-c-...")]
		public double sub()
		{
			var arguments = this.getArgumentsOfType<float>();
			double sub = Convert.ToDouble(arguments[0]);
			for (int i = 1; i < arguments.Length; i++)
				sub = sub - Convert.ToDouble(arguments[i]);
			return sub;
		}


		[Image("multiplies float a, float b, float c, ... : double")]
		[@Description("Equivalent a*b*c*...")]
		public double mul()
		{
			var arguments = this.getArgumentsOfType<float>();
			double sub = Convert.ToDouble(arguments[0]);
			for (int i = 1; i < arguments.Length; i++)
				sub = sub * Convert.ToDouble(arguments[i]);
			return sub;
		}
	}

}