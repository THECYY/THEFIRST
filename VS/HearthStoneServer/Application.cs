using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using System.IO;
using log4net.Config;
using HearthStoneServer.NHibernateManager;
using Allpurpose.Emun;
using HearthStoneServer.Entity;
using Allpurpose.Enum;
using Allpurpose.Utils;
using HearthStoneServer.Handler;

namespace HearthStoneServer
{
    public class Application : ApplicationBase
    {

        //日志文件对象
        public static ILogger logger = LogManager.GetCurrentClassLogger();

        //Handler字典
        public static Dictionary<OPCode, BaseHandler> handlerDict = new Dictionary<OPCode, BaseHandler>();

        //应用根目录
        public static String rootPath;

        //登录的user字典
        public static Dictionary<String, User> loginUserDict = new Dictionary<string, User>();

        //已登录的MyClientPeer字典
        public static Dictionary<String, MyClientPeer> clientPeerDict = new Dictionary<string, MyClientPeer>();

        //已经准备的UserNameList
        public static List<String> readyList = new List<string>();

        //正在进行邀请的Dictionary
        public static Dictionary<String, String> Waiting = new Dictionary<string, string>();

        //正在进行对战的Dictionary
        public static Dictionary<String, String> Fighting = new Dictionary<string, string>();

        //客户端连接的时候
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            logger.Info("====================================HearthStone客户端连接成功================================================");
            MyClientPeer peer = new MyClientPeer(initRequest);
            return peer;
        }

        //Handler初始化函数
        private void initHandler() {
            LoginHandler loginHandler = new LoginHandler();
            handlerDict.Add(loginHandler.opCode, loginHandler);
            RegisterHandler registerHandler = new RegisterHandler();
            handlerDict.Add(registerHandler.opCode, registerHandler);
            PurchaseCardPackageHandler purchaseCardPackageHandler = new PurchaseCardPackageHandler();
            handlerDict.Add(purchaseCardPackageHandler.opCode, purchaseCardPackageHandler);
            UseCardPackageHandler useCardPackageHandler = new UseCardPackageHandler();
            handlerDict.Add(useCardPackageHandler.opCode, useCardPackageHandler);
            ManufactureCardHandler manufactureCardHandler = new ManufactureCardHandler();
            handlerDict.Add(manufactureCardHandler.opCode, manufactureCardHandler);
            DecomposeCardHandler decomposeCardHandler = new DecomposeCardHandler();
            handlerDict.Add(decomposeCardHandler.opCode, decomposeCardHandler);
            NewCardsSetHandler newCardsSetHandler = new NewCardsSetHandler();
            handlerDict.Add(newCardsSetHandler.opCode, newCardsSetHandler);
            ChangeCardsSetHandler changeCardsSetHandler = new ChangeCardsSetHandler();
            handlerDict.Add(changeCardsSetHandler.opCode, changeCardsSetHandler);
            DeleteCardsSetHandler deleteCardsSetHandler = new DeleteCardsSetHandler();
            handlerDict.Add(deleteCardsSetHandler.opCode, deleteCardsSetHandler);
            ReadyHandler readyHandler = new ReadyHandler();
            handlerDict.Add(readyHandler.opCode, readyHandler);
            LeftHandler leftHandler = new LeftHandler();
            handlerDict.Add(leftHandler.opCode, leftHandler);
            FightInvitHandler fightInvitHandler = new FightInvitHandler();
            handlerDict.Add(fightInvitHandler.opCode, fightInvitHandler);
            FightInvitResponseHandler fightInvitResponseHandler = new FightInvitResponseHandler();
            handlerDict.Add(fightInvitResponseHandler.opCode, fightInvitResponseHandler);
        }

        //服务器开始的时候
        protected override void Setup()
        {
            //日志初始化操作
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "bin_Win64\\log");
            FileInfo logConfigFileinfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));
            if (logConfigFileinfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(logConfigFileinfo);
            }
            logger.Info("=======================================HearthStone服务器开启===========================================");

            //传递应用根路径
            Application.rootPath = this.ApplicationRootPath;

            //所有Handler初始化
            initHandler();
        }

        //服务器关闭的时候
        protected override void TearDown()
        {
            logger.Info("=======================================HearthStone服务器关闭=====================================================");
        }
    }
}
