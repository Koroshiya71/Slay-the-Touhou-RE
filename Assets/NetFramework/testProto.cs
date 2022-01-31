using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class testProto : MonoBehaviour {


	// Use this for initialization
	void Start () {


		//Json测试
		MsgMove msgMove = new MsgMove();
		byte[] bs = MsgBase.EncodeName(msgMove);

		int count;
		string name = MsgBase.DecodeName(bs, 0, out count);
		Debug.Log(name); 
		Debug.Log(count);



	}


}
