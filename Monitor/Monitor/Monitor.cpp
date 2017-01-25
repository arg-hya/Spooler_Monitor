// monitor_file.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"


#include <windows.h>
#include <wincon.h>
#include <winspool.h>
#include <time.h>
#include <stdio.h>
#include <stdlib.h>
#include<iostream>
#include <fstream>


HANDLE hPrinter = INVALID_HANDLE_VALUE;

time_t start, end;

char  portname[MAX_PATH] = { '\0' };

void GetDefaultPrinterName()
{
	DWORD length = MAX_PATH;
	GetDefaultPrinter(portname, &length);

}

int main(){

	HANDLE hwndNotify = 0;
	float t;
	int count = 0;

	GetDefaultPrinterName();

	std::ofstream outfile;
	std::string userProfile = getenv("userprofile");
	userProfile.append("\\Documents\\Result.txt");
	outfile.open(userProfile);
	std::cout << " Started";
	outfile << "Default printer is : " << portname << std::endl;

	OpenPrinter(portname, &hPrinter, NULL);

	if (hPrinter == 0){
		printf("Crap!\n");
		system("PAUSE");
		return 0;
	}


	if ((hwndNotify = FindFirstPrinterChangeNotification(hPrinter, PRINTER_CHANGE_JOB, 0, NULL)) != INVALID_HANDLE_VALUE)
	{
		while (hwndNotify != INVALID_HANDLE_VALUE)
		{
			DWORD  Changes = 0x00000000;
			if (FindNextPrinterChangeNotification(hwndNotify, &Changes, NULL, NULL))
			{
				if (Changes & PRINTER_CHANGE_ADD_JOB)
				{
					start = clock();
					outfile << "Job No: " << count << std::endl;
					outfile << "Job entered spooler " << std::endl;
					count++;
				}
				else if (Changes & PRINTER_CHANGE_DELETE_JOB)
				{
					end = clock();
					t = ((float)(end - start)) / CLOCKS_PER_SEC;
					outfile << "Job Deleted  Time taken = " << t << std::endl;
					//FindClosePrinterChangeNotification(hwndNotify);

					std::cout << "\n\n Job Ended";

				}
				
				else if (Changes & PRINTER_CHANGE_WRITE_JOB)
				{
					end = clock();
					t = ((float)(end - start)) / CLOCKS_PER_SEC;
					outfile << "Job Written  Time taken =  " << t << std::endl;
				}
			}
		}
	}

	return 0;
}

