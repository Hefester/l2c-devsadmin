﻿namespace L2.Net.Structs.Client
{
    public static class KeyPacket
    {
        private static readonly Packet m_Prepared = new OPPacket(0x2e);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cryptKey"></param>
        /// <returns></returns>
        public static Packet Create( byte[] cryptKey )
        {
            Packet p = m_Prepared;
            p.WriteByte(0x01);
            p.WriteBytesArray(cryptKey);
            p.WriteInt(1, 0);
            p.WriteByte(0);
            p.WriteInt(0);
            return p;
        }
    }
}
