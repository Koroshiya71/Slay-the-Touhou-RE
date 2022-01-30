using System;

namespace Game
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if(!DbManager.Connect("game", "127.0.0.1", 3306, "root", "")){
				return;
			}

			NetManager.StartLoop(8888);
		}
	}
}
