using System;
using System.Collections.Generic;
using System.IO;

namespace NameGenerateProject {
	class Program {
		static void Main(string[] args) {

			var text = File.ReadAllText(@"E:\Jenocn\NameGenerate\name_word_lib.json");

			var nameGenerate = new NameGenerate();
			nameGenerate.Parse(text);
			for (int i = 0; i < 20; ++i) {
				var ret = nameGenerate.Generate("", 0, 0, 0, new List<NameWordFeatureType>());
				Console.Write("[");
				Console.Write(ret.Item1 + " " + (ret.Item2 == 1 ? "男" : "女"));
				Console.Write("],  ");
			}
			Console.WriteLine();
		}
	}
}
