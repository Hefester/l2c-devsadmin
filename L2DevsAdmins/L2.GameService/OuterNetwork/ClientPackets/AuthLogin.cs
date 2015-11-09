using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2.Net.GameService.OuterNetwork.ClientPackets
{
    internal class AuthLogin : Packet
    {
        private String _loginName;
        private int _playKey1;
        private int _playKey2;
        private int _loginKey1;
        private int _loginKey2;
        private Packet packet;

        public AuthLogin()
        {
            _loginName = ReadString();
            _playKey2 = ReadInt();
            _playKey1 = ReadInt();
            _loginKey1 = ReadInt();
            _loginKey2 = ReadInt();
        }

        public AuthLogin(Packet packet)
        {
            this.packet = packet;
        }

        public override void RunImpl()
        {
            Logger.WriteLine(Source.Debug, "");
            Logger.WriteLine(Source.Debug, "_loginName = {0}", _loginName);
            Logger.WriteLine(Source.Debug, "_playKey2 = {0}", _playKey2);
            Logger.WriteLine(Source.Debug, "_playKey1 = {0}", _playKey1);
            Logger.WriteLine(Source.Debug, "_loginKey1 = {0}", _loginKey1);
            Logger.WriteLine(Source.Debug, "_loginKey2 = {0}", _loginKey2);
            Logger.WriteLine(Source.Debug, "");
        }

        public override string GetPacketName()
        {
            return "SSS AuthLogin";
        }
    }
}
