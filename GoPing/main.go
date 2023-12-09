package main

import (
	"bufio"
	"encoding/json"
	"fmt"
	"net"
	"os"
	"strconv"
	"time"

	"github.com/inancgumus/screen"
)

type PingData struct {
	Ip          string
	PingTime    time.Time
	PingLatency int64
	Error       string
	IsSuccess   bool
}

const PingCommand = "p"
const StopCommand = "s"

func main() {

	if len(os.Args) < 3 {
		PrintError("Need {ping target} {port} as arguments")
		return
	}

	Port, perr := strconv.Atoi(os.Args[2])
	if perr != nil {
		PrintError("Incorrect port argument")
		return
	}

	var latencyMs int64 = 0
	var ip net.IP = net.ParseIP(os.Args[1])

	if ip == nil {
		ips, iperr := net.LookupIP(os.Args[1])
		if iperr != nil {
			PrintError(iperr.Error())
		} else {
			ip = ips[0]
		}
	}

	if ip != nil {
		scanner := bufio.NewScanner(os.Stdin)
		for scanner.Scan() {
			if scanner.Text() == StopCommand {
				break
			} else if scanner.Text() == PingCommand {
				screen.Clear()
				screen.MoveTopLeft()
				var tstartping = time.Now()
				d := net.Dialer{Timeout: time.Second * 10}
				pingError := ""
				conn, err := d.Dial("tcp4", fmt.Sprintf("%s:%d", ip, Port))
				if err != nil {
					pingError = err.Error()
				} else {
					conn.Close()

					latencyMs = time.Since(tstartping).Milliseconds()
				}
				PrintPing(ip.String(), tstartping, latencyMs, pingError)
			}
		}

	} else {
		PrintError("Invalid ping target address")
	}

}

func PrintPing(ip string, startTime time.Time, latencyMs int64, errormessage string) {
	m := PingData{Ip: ip, PingTime: startTime, PingLatency: latencyMs, Error: errormessage, IsSuccess: errormessage == ""}
	b, jerr := json.Marshal(m)
	if jerr == nil {
		fmt.Printf("%s\n", b)
	} else {
		PrintError(jerr.Error())
	}
}

func PrintError(errormessage string) {
	fmt.Printf("%s\n", errormessage)
}
