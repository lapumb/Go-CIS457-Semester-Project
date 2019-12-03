import socket
import threading
import os
import xml.etree.ElementTree as ET
from os import listdir, path


PORT = 12000
host_name = socket.gethostname() 
IP = socket.gethostbyname(host_name) 
#IP = "127.0.0.1"
SERVER_PORTS = 18397
userDict = {}
print("IP: " + IP)

class Client(threading.Thread):
    def __init__(self, s, addr):
        self.request = s
        self.addr = addr
        self.port = addr[1]
        super(Client, self).__init__()

    def run(self):
        global SERVER_PORTS
        while (True):
            try:
                cmd = self.request.recv(1024).decode('utf-8')
                command = cmd.split()
                print(command)
                if command[0] == "QUIT":
                    self.quit(command[1])
                    return
               # port = int(command[len(command) - 1])
                print("connecting...")
                if command[0] == "MOVE":
                    self.playGame(self.request, cmd)
                if command[0] == "CONNECT":
                    #s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                    #s.connect((IP, port))
                    userName = self.storeUsers(command[1], SERVER_PORTS, self.request)
                    while(True):
                        opponent = self.connectToOpponent(userName)
                        if(opponent):
                            opponentPort = int(opponent.split()[1])
                            myPort = self.port
                            print("opponent port: " + str(opponentPort))
                            print("my port: " + str(self.port))
                            if(opponentPort > myPort):
                                opponent += " " + "1"
                            else:
                                opponent += " " + "0"
                            self.request.send(opponent.encode('utf-8'))
                            break
                    #self.request.send(("ACK CONNECT " + str(SERVER_PORTS)).encode('utf-8'))
                    SERVER_PORTS = SERVER_PORTS + 2
                    continue
                #s.close()
            except socket.error as exc:
                print("Connection error: " + str(exc))

    def playGame(self, sock, cmd):
        cmdSplit = cmd.split()
        #find opponent
        opponent = userDict.get(cmdSplit[1])
        if(opponent):
            oppSock = opponent[1]
            print("opponent socket: " + str(oppSock))
            #send move to opponent
            oppSock.send(cmd.encode('utf-8'))

    def quit(self, userName):
        self.deleteUser(userName)
        print("Client Has Disconnected")
        self.request.close()

    def storeUsers(self, username, portNumber, socket):
        username += " " + str(self.port)
        print(username)
        #portNumber, socket, playing
        userInfo = [portNumber, socket, "false"]
        userDict[username] = userInfo
        return username
        
    def deleteUser(self, username):
        userDict.pop(username, None)

    def connectToOpponent(self, userName):
        for name, lst in userDict.items():
            #find a user who is not in a current match and is not the current user
            if(lst[2] == "false" and name != userName):
                lst[2] = "true"
                return str(name)
        
serv = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
serv.bind((IP, PORT))
serv.listen(1)

while True:
    conn, addr = serv.accept()
    print("USER: " + str(addr) + " CONNECTED")
    client_thr = Client(conn, addr)
    client_thr.start()
