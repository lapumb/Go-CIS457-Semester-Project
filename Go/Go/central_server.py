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
fileList = []
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
                command = self.request.recv(1024).decode('utf-8').split()
                print(command)
                if command[0] == "QUIT":
                    self.quit(command[1])
                    return
                port = int(command[len(command) - 1])
                print("connecting...")
                if command[0] == "CONNECT":
                    self.storeUsers(command[1], command[2], SERVER_PORTS, command[3])
                    print("in connected")
                    self.request.send(("ACK CONNECT " + str(SERVER_PORTS)).encode('utf-8'))
                    SERVER_PORTS = SERVER_PORTS + 2
                    continue
                s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                s.connect((IP, port))
                if command[0] == "FILEDESC":
                    fileName = command[1]
                    s.send(("ACK FILEDESC " + fileName).encode('utf-8'))
                    self.stor(s, command[1])
                    self.parseXML(s, command[1], command[2])
                elif command[0] == "SEARCH":
                    s.send(("ACK SEARCH").encode('utf-8'))
                    self.search(command[1], command[2], s)
                s.close()
            except socket.error as exc:
                print("Connection error: " + str(exc))
                    
            #CONNECT Username Hostname ConnectionSpeed
            #Send ACK command name
            #FILEDESC FileName Username
            #SEARCH DESCR Username
            #LOCATION list
            #QUIT Username
            #wait for client to send username, hostname, and connection speed
            #store username, hostname, and connection speed in a table/list
            #send acknowledgement back to client
            #wait for client to send xml file with shared file descriptions
            #parse xml file and store descriptions in a table/list
            #wait for user to send a keyword search

    def stor(self, s, fileName):
        f = open(fileName, "w")
        print("Created file " + fileName)
        line = s.recv(1024).decode('utf-8')
        while line:
            f.write(line)
            line = s.recv(1024).decode('utf-8')
        f.close()
        print("File Downloaded")

    def parseXML(self, s, fileName, userName):
        if path.exists(fileName):
            tree = ET.parse(fileName)
            root = tree.getroot()
            for child in root:
                name = child[0].text
                descr = child[1].text
                self.storeFiles(userName, name, descr)
            print("Successfully parsed XML file")
            os.remove(fileName)
        else:
            print("Parse Failed")
            s.send("Parse Failed".encode('utf-8'))

    def quit(self, userName):
        self.deleteUser(userName)
        print("Client Has Disconnected")
        self.request.close()

    def storeUsers(self, username, hostName, portNumber, connSpeed):
        userInfo = [hostName, portNumber, connSpeed]
        userDict[username] = userInfo

    def storeFiles(self, userName, fileName, description):
        fileInfo = [userName, fileName, description]
        fileList.append(fileInfo)
        
    def deleteUser(self, username):
        global fileList
        fileList = [f for f in fileList if f[0] != username]
        userDict.pop(username, None)

    def search(self, srchString, userName, s):
        #Let the client know that we are running the search so that it knows what to branch to
        s.recv(1024).decode('utf-8')

        #Goes through each file that is stores for all users
        #fileList is an array of arrays formatted as [[username, filename, descr] [username...]]
        for file in fileList:
            fileName = file[1]
            desc = file[2]
            if srchString in fileName or srchString in desc:
                #userDict is a dictionary of arrays of hostname, port number, connSpeed (like fileList)
                user = userDict.get(file[0])
                rtrnString = user[0] + " " + str(user[1]) + " " + file[1] + " " + user[2]    #hostname port fileName connSpeed
                s.send(rtrnString.encode('utf-8'))
                s.recv(1024).decode('utf-8')
        s.send("EOF SEARCH".encode('utf-8'))
        
serv = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
serv.bind((IP, PORT))
serv.listen(1)

while True:
    conn, addr = serv.accept()
    print("USER: " + str(addr) + " CONNECTED")
    client_thr = Client(conn, addr)
    client_thr.start()
