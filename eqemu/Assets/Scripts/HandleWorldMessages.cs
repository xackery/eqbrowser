using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

namespace EQBrowser
{
	public partial class WorldConnect : MonoBehaviour
	{
		/* UI Events Below */



		public void DoEnterWorld(string name)
		{
			
			if (CSel == null)
				return;

			ourPlayerName = name;
			AttemptingZoneConnect = true;
			byte[] EnterWorldRequest = new byte[72];
			int position = 0;

			WriteFixedLengthString(name, ref EnterWorldRequest, ref position,  64); //charname
			WriteInt32(0, ref EnterWorldRequest, ref position); // enter tutorial
			WriteInt32(0, ref EnterWorldRequest, ref position); // return home
			GenerateAndSendWorldPacket (EnterWorldRequest.Length, 151 /* OP_EnterWorld */, -1, -1, EnterWorldRequest);				
		}

		public void DoNameApproval()
		{

			if (CSel == null)
				return;

			if (CSel.CreationName.text == "") {
				CSel.CreationStatus.text = "Please enter a character name.";
				return;
			}

			byte[] NameApprovalRequest = new byte[72];
			int position = 0;
			WriteFixedLengthString(CSel.CreationName.text, ref NameApprovalRequest, ref position,  64);
			WriteInt32(CSel._RaceSelection, ref NameApprovalRequest, ref position);
			WriteInt32(CSel._ClassSelection, ref NameApprovalRequest, ref position);
			GenerateAndSendWorldPacket (NameApprovalRequest.Length, 36 /* OP_CharacterCreate */, -1, -1, NameApprovalRequest);		
		}


		/* Incoming packet handlers below */


		//545
		public void HandleWorldMessage_ZoneServerInfo(byte[] data, int datasize)
		{
			//ignore packet contents, browser doesn't use this, instead send request based on selected character
			byte[] ZoneEntryRequest = new byte[68];
			Int32 pos = 0;

			curZoneId = 2;

			WriteInt32 (0, ref ZoneEntryRequest, ref pos);
			WriteFixedLengthString(ourPlayerName, ref ZoneEntryRequest, ref pos, 64);
			GenerateAndSendWorldPacket (ZoneEntryRequest.Length, 541, curZoneId, curInstanceId, ZoneEntryRequest);
			AttemptingZoneConnect = false;
		}

		//548
		public void HandleWorldMessage_ZoneUnavailable(byte[] data, int datasize)
		{
			AttemptingZoneConnect = false;
		}

		//365
		public void HandleWorldMessage_PlayerProfile(byte[] data, int datasize)
		{
			byte[] NewZoneRequest = null;
			GenerateAndSendWorldPacket (0, 403 /* OP_ReqNeqZone */, curZoneId, curInstanceId, NewZoneRequest);
		}
		//338
		public void HandleWorldMessage_NewZone(byte[] data, int datasize)
		{
			byte[] ReqClientSpawn = null;
			GenerateAndSendWorldPacket (0, 402 /* OP_ReqClientSpawn */, curZoneId, curInstanceId, ReqClientSpawn);
		}
		//
		public void HandleWorldMessage_ZoneServerReady(byte[] data, int datasize)
		{
			byte[] NotifyClientReady = null;
			GenerateAndSendWorldPacket (0, 85 /* OP_ClientReady */, curZoneId, curInstanceId, NotifyClientReady);
		}

		public void HandleWorldMessage_EmuKeepAlive(byte[] data, int datasize)
		{
			byte[] KeepAlive = null;
			GenerateAndSendWorldPacket (0, 549 /* OP_EmuKeepAlive */, curZoneId, curInstanceId, KeepAlive);
		}

		//36
		public void HandleWorldMessage_ApproveName(byte[] data, int datasize)
		{
			if (datasize != 1 || CSel == null)
				return;
			int position = 0;
			byte result = ReadInt8 (data, ref position);

			if (result == 1) {
				//Create character. Our params are as follows:
				byte[] CharCreateRequest = new byte[92];
				Int32 pos = 0;

				WriteInt32(CSel._ClassSelection, ref CharCreateRequest, ref pos);
				WriteInt32(255, ref CharCreateRequest, ref pos); //haircolor
				WriteInt32(CSel.BeardSelection, ref CharCreateRequest, ref pos);
				WriteInt32(255, ref CharCreateRequest, ref pos); //beard color
				WriteInt32(CSel.GenderSelection, ref CharCreateRequest, ref pos);
				WriteInt32(CSel._RaceSelection, ref CharCreateRequest, ref pos);
				WriteInt32(CSel.ZoneSelection, ref CharCreateRequest, ref pos);
				WriteInt32(CSel.HairSelection, ref CharCreateRequest, ref pos);
				WriteInt32(CSel._DeitySelection, ref CharCreateRequest, ref pos);
				WriteInt32(CSel._ClassSelection, ref CharCreateRequest, ref pos);
				WriteInt32(75, ref CharCreateRequest, ref pos); //STR
				WriteInt32(75, ref CharCreateRequest, ref pos); //STA
				WriteInt32(75, ref CharCreateRequest, ref pos); //DEX
				WriteInt32(75, ref CharCreateRequest, ref pos); //AGI
				WriteInt32(75, ref CharCreateRequest, ref pos); //INT
				WriteInt32(75, ref CharCreateRequest, ref pos); //WIS
				WriteInt32(75, ref CharCreateRequest, ref pos); //CHA
				WriteInt32(0, ref CharCreateRequest, ref pos); //Face
				WriteInt32(255, ref CharCreateRequest, ref pos); //Eye color
				WriteInt32(0, ref CharCreateRequest, ref pos); //Drakkin Heritage
				WriteInt32(0, ref CharCreateRequest, ref pos); //Drakkin Tattoo
				WriteInt32(0, ref CharCreateRequest, ref pos); //Drakkin Details
				WriteInt32(CSel.TutorialSelection, ref CharCreateRequest, ref pos); //Tutorial is selected?
				GenerateAndSendWorldPacket (CharCreateRequest.Length, 70 /* OP_CharacterCreate */, -1, -1, CharCreateRequest);		
			
			}


		}
		
		//423
		public void HandleWorldMessage_SendCharInfo(byte[] data, int datasize)
		{
			
			if (datasize <= 0 || CSel == null)
				return;
			
			
			CSel.ClearCharButtonText ();
			Int32 position = 0;
			Int32 numChar = ReadInt32(data, ref position);
			Int32 numAllowedChar = ReadInt32(data, ref position);
			int i = 0;
			int curSelIndex = 0;
			for (i = 0; i < numChar; i++) {
				string name = ReadFixedLengthString(data, ref position, 64);
				byte _class = ReadInt8(data, ref position);
				Int32 _race = ReadInt32(data, ref position);
				byte level = ReadInt8(data, ref position);
				byte _shroudclass = ReadInt8(data, ref position);
				Int32 _shroudrace = ReadInt32(data, ref position);
				Int16 zoneid = ReadInt16(data, ref position);
				Int16 instanceid = ReadInt16(data, ref position);
				byte gender = ReadInt8(data, ref position);
				byte face = ReadInt8(data, ref position);
				int j = 0;
				for(j = 0; j < 9; j++)
				{
					Int32 Material1 = ReadInt32 (data, ref position);
					Int32 Material2 = ReadInt32 (data, ref position); 
					byte Red = ReadInt8 (data, ref position); 
					byte Green = ReadInt8 (data, ref position); 
					byte Blue = ReadInt8 (data, ref position);
					byte tint = ReadInt8 (data, ref position);
				}
				Int32 deity = ReadInt32(data, ref position);
				Int32 idfile1 = ReadInt32(data, ref position);
				Int32 idfile2 = ReadInt32(data, ref position);
				byte haircolor = ReadInt8(data, ref position);
				byte beardcolor = ReadInt8(data, ref position);
				byte eyecolor1 = ReadInt8(data, ref position);
				byte eyecolor2 = ReadInt8(data, ref position);
				byte hairstyle = ReadInt8(data, ref position);
				byte beard = ReadInt8(data, ref position);
				byte gohome = ReadInt8(data, ref position);
				byte tutorial = ReadInt8(data, ref position);
				Int32 DrakkinHeritage = ReadInt32(data, ref position);
				byte enabled = ReadInt8(data, ref position);
				Int32 lastlogin = ReadInt32(data, ref position);
				if(_class > 0)
				{
					CSel.UpdateCharButtonText(curSelIndex, name);
					curSelIndex++;
				}

			}
			CSel.ToCharList();
			
		}

	}
}

